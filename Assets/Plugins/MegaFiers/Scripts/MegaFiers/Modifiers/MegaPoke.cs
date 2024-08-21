using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	public enum Falloff
	{
		None,
		Curve,
		Sharp,
		Smooth,
		Root,
		Linear,
		Const,
		Sphere,
		InvSquare,
	}

	[AddComponentMenu("Modifiers/Poke")]
	public class MegaPoke : MegaModifier
	{
		[Adjust]
		public Transform		fromObj;
		[Adjust]
		public Transform		toObj;
		[Adjust]
		public bool				preserveVol;
		[Adjust]
		public float			strength			= 1.0f;
		[Adjust]	
		public Falloff			falloffType			= Falloff.Smooth;
		[Adjust]
		public float			radius				= 1.0f;
#if false
		[Adjust]
		public Transform		pullObj;
		[Adjust]
		public float			pullStrength		= 1.0f;
		[Adjust]
		public float			pullRadius			= 1.0f;
#endif
		public AnimationCurve	curve				= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		float					falloff_radius_sq	= 0.0f;
		Job						job;
		JobHandle				jobHandle;
		float					weight				= 0.0f;
		Vector3					fromlocpos;
		Matrix4x4				mat_from;
		Matrix4x4				mat_to;
		Matrix4x4				mat_from_inv;
		Matrix4x4				mat_final;
		Matrix4x4				tmat;
		Matrix4x4				mat_unit			= Matrix4x4.identity;

		public override string			ModName()			{ return "Poke"; }
		public override string			GetHelpURL()		{ return "?page_id=168"; }
		public override MegaModChannel	ChannelsReq()		{ return MegaModChannel.Verts; }
		public override MegaModChannel	ChannelsChanged()	{ return MegaModChannel.Verts; }

		[ContextMenu("Init")]
		public virtual void Init()
		{
			MegaModifyObject mod = (MegaModifyObject)GetComponent<MegaModifyObject>();
		}

		public override void MeshChanged()
		{
			Init();
		}

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public float				weight;
			public float				falloff_radius_sq;
			public float				radius;
			public bool					preserveVol;
			public Vector3				fromlocpos;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			mat_from;
			public Matrix4x4			mat_from_inv;
			public Matrix4x4			mat_final;
			public Matrix4x4			mat_unit;
			public Matrix4x4			tmat;
			public Falloff				falloffType;
			[Unity.Collections.ReadOnly]
			public animationcurve		curve;

			void Interp(ref Matrix4x4 outm, Matrix4x4 dst, Matrix4x4 src, float srcweight)
			{
				float4 c2 = src.GetColumn(2);
				float4 c1 = src.GetColumn(1);
				float4 d2 = dst.GetColumn(2);
				float4 d1 = dst.GetColumn(1);

				quaternion squat = quaternion.LookRotation(c2.xyz, c1.xyz);
				quaternion dquat = quaternion.LookRotation(d2.xyz, d1.xyz);

				Vector3 sloc = src.GetColumn(3);
				Vector3 dloc = dst.GetColumn(3);

				Vector3 sscale = src.lossyScale;
				Vector3 dscale = dst.lossyScale;

				Vector3 floc = math.lerp(dloc, sloc, srcweight);
				quaternion fquat = math.slerp(dquat, squat, srcweight);
				Vector3 fsize = math.lerp(dscale, sscale, srcweight);

				outm.SetTRS(floc, fquat, fsize);
			}

			public void Execute(int i)
			{
				Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

				float fac = (p - fromlocpos).sqrMagnitude;

				if ( falloffType == Falloff.None || fac < falloff_radius_sq )   //&& (fac = (radius - Mathf.Sqrt(fac)) / radius)) )
				{
					fac = (radius - math.sqrt(fac)) / radius;

					switch ( falloffType )
					{
						case Falloff.None:		fac = 1.0f; break;
						case Falloff.Curve:		fac = curve.Evaluate(fac); break;
						case Falloff.Sharp:		fac = fac * fac; break;
						case Falloff.Smooth:	fac = 3.0f * fac * fac - 2.0f * fac * fac * fac; break;
						case Falloff.Root:		fac = math.sqrt(fac); break;
						case Falloff.Linear:     /* pass */  break;
						case Falloff.Const:		fac = 1.0f; break;
						case Falloff.Sphere:	fac = math.sqrt(2.0f * fac - fac * fac); break;
						case Falloff.InvSquare: fac = fac * (2.0f - fac); break;
					}

					fac *= weight;
					if ( fac != 0.0f )
					{
						p = mat_from_inv.MultiplyPoint(p);

						if ( fac == 1.0f )
							p = mat_final.MultiplyPoint(p);
						else
						{
							if ( preserveVol )
							{
								Interp(ref tmat, mat_unit, mat_final, fac);
								p = tmat.MultiplyPoint(p);
							}
							else
							{
								Vector3 tvec = mat_final.MultiplyPoint(p);
								p = math.lerp(p, tvec, fac);
							}
						}

						p = mat_from.MultiplyPoint(p);
					}
				}
				jsverts[i] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )	//&& map != null )
			{
				job.weight = weight;
				job.radius = radius;
				job.falloff_radius_sq = falloff_radius_sq;
				job.falloffType = falloffType;
				job.preserveVol = preserveVol;
				job.fromlocpos = fromlocpos;

				job.tm = tm;
				job.invtm = invtm;
				job.mat_from = mat_from;
				job.mat_from_inv = mat_from_inv;
				job.mat_final = mat_final;
				job.mat_unit = mat_unit;

				job.jvertices = jverts;
				job.jsverts = jsverts;

				//if ( falloffType == Falloff.Curve )
					job.curve.keys = new NativeArray<Keyframe>(curve.keys, Allocator.TempJob);    // only need to set once

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();

				if ( job.curve.keys.IsCreated )
					job.curve.keys.Dispose();
			}
		}

		float	lastRadius;
		float	lastWeight;
		float	lastStrength;
		bool	lastPreserve;
		Falloff lastFalloff;
		Vector3	lastFromPos;
		Vector3 lastToPos;

		public override bool Changed()
		{
			if ( lastRadius != radius )
			{
				lastRadius = radius;
				return true;
			}

			if ( lastWeight != weight )
			{
				lastWeight = weight;
				return true;
			}

			if ( lastStrength != strength )
			{
				lastStrength = strength;
				return true;
			}

			if ( lastPreserve != preserveVol )
			{
				lastPreserve = preserveVol;
				return true;
			}

			if ( lastFalloff != falloffType )
			{
				lastFalloff = falloffType;
				return true;
			}

			Vector3 pos = transform.InverseTransformPoint(fromObj.position);
			if ( lastFromPos != pos )
			{
				lastFromPos = pos;
				return true;
			}

			pos = transform.InverseTransformPoint(toObj.position);
			if ( lastToPos != pos )
			{
				lastToPos = pos;
				return true;
			}

			return false;
		}

		void Interp(ref Matrix4x4 outm, Matrix4x4 dst, Matrix4x4 src, float srcweight)
		{
			Quaternion squat = Quaternion.LookRotation(src.GetColumn(2), src.GetColumn(1));
			Quaternion dquat = Quaternion.LookRotation(dst.GetColumn(2), dst.GetColumn(1));

			Vector3 sloc = src.GetColumn(3);
			Vector3 dloc = dst.GetColumn(3);

			Vector3 sscale = src.lossyScale;
			Vector3 dscale = dst.lossyScale;

			Vector3 floc = Vector3.Lerp(dloc, sloc, srcweight);
			Quaternion fquat = Quaternion.Slerp(dquat, squat, srcweight);
			Vector3 fsize = Vector3.Lerp(dscale, sscale, srcweight);

			outm.SetTRS(floc, fquat, fsize);
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			float fac = (p - fromlocpos).sqrMagnitude;

			if ( falloffType == Falloff.None || fac < falloff_radius_sq )	//&& (fac = (radius - Mathf.Sqrt(fac)) / radius)) )
			{
				fac = (radius - Mathf.Sqrt(fac)) / radius;

				switch ( falloffType )
				{
					case Falloff.None:		fac = 1.0f; break;
					case Falloff.Curve:		fac = curve.Evaluate(fac); break;
					case Falloff.Sharp:		fac = fac * fac; break;
					case Falloff.Smooth:	fac = 3.0f * fac * fac - 2.0f * fac * fac * fac; break;
					case Falloff.Root:		fac = Mathf.Sqrt(fac); break;
					case Falloff.Linear:     /* pass */  break;
					case Falloff.Const:		fac = 1.0f; break;
					case Falloff.Sphere:	fac = Mathf.Sqrt(2.0f * fac - fac * fac); break;
					case Falloff.InvSquare:	fac = fac * (2.0f - fac); break;
				}

				fac *= weight;

				if ( fac != 0.0f )
				{
					p = mat_from_inv.MultiplyPoint(p);

					if ( fac == 1.0f )
						p = mat_final.MultiplyPoint(p);
					else
					{
						if ( preserveVol )
						{
							Interp(ref tmat, mat_unit, mat_final, fac);
							p = tmat.MultiplyPoint(p);
						}
						else
						{
							Vector3 tvec = mat_final.MultiplyPoint(p);
							p = Vector3.Lerp(p, tvec, fac);
						}
					}

					p = mat_from.MultiplyPoint(p);
				}
			}

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			if ( fromObj && toObj )
			{
				fromlocpos = transform.InverseTransformPoint(fromObj.position);

				weight = strength;
				Matrix4x4 obvinv = transform.worldToLocalMatrix;	//invert_m4_m4(obinv, ob->obmat);

				/* Checks that the objects/bones are available. */
				mat_from = obvinv * fromObj.localToWorldMatrix;	//(mat_from, obinv, wmd->object_from, wmd->bone_from);
				mat_to = obvinv * toObj.localToWorldMatrix;	//(mat_to, obinv, wmd->object_to, wmd->bone_to);

				tmat = mat_from.inverse;
				mat_final = tmat * mat_to;

				mat_from_inv = mat_from.inverse;
				mat_unit = Matrix4x4.identity;

				if ( strength < 0.0f )
				{
					weight = -strength;
					Vector3 loc = mat_final.GetColumn(3);
					mat_final = mat_final.inverse;

					mat_final.SetColumn(3, -loc);
				}

				falloff_radius_sq = radius * radius;
			}
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( fromObj && toObj )
				return true;

			return false;
		}
	}
}