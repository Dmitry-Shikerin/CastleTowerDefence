using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Radial Skew")]
	public class MegaRadialSkew : MegaModifier
	{
		[Adjust]
		public float	angle		= 0.0f;
		[Adjust]
		public MegaAxis	axis		= MegaAxis.X;
		[Adjust]
		public MegaAxis	eaxis		= MegaAxis.X;
		[Adjust]
		public bool		biaxial		= false;
		float			skewamount;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "RaidalSkew"; }
		public override string GetHelpURL() { return "?page_id=305"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				skewamount;
			public bool					biaxial;
			public MegaAxis				eaxis;
			public MegaAxis				axis;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			float3 GetSkew(float3 p)
			{
				if ( biaxial )
				{
					switch ( axis )
					{
						case MegaAxis.X:
							switch ( eaxis )
							{
								case MegaAxis.Y: p.x = p.z = 0.0f; break;
								case MegaAxis.Z: p.x = p.y = 0.0f; break;
								default: p.x = p.y = 0.0f; break;
							}
							break;

						case MegaAxis.Y:
							switch ( eaxis )
							{
								case MegaAxis.X: p.y = p.z = 0.0f; break;
								case MegaAxis.Z: p.x = p.y = 0.0f; break;
								default: p.x = p.y = 0.0f; break;
							}
							break;

						case MegaAxis.Z:
							switch ( eaxis )
							{
								case MegaAxis.X: p.y = p.z = 0.0f; break;
								case MegaAxis.Y: p.x = p.z = 0.0f; break;
								default: p.y = p.z = 0.0f; break;
							}
							break;
					}
				}
				else
				{
					switch ( axis )
					{
						case MegaAxis.X: p.x = 0.0f; break;
						case MegaAxis.Y: p.y = 0.0f; break;
						case MegaAxis.Z: p.z = 0.0f; break;
					}
				}

				return math.normalize(p);
			}

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float3 skewv = GetSkew(p) * skewamount * p[(int)axis];

				jsverts[vi] = invtm.MultiplyPoint3x4(p + skewv);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.skewamount	= skewamount;
				job.axis		= axis;
				job.eaxis		= eaxis;
				job.biaxial		= biaxial;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		Vector3 GetSkew(Vector3 p)
		{
			if ( biaxial )
			{
				switch ( axis )
				{
					case MegaAxis.X:
						switch ( eaxis )
						{
							case MegaAxis.Y: p.x = p.z = 0.0f; break;
							case MegaAxis.Z: p.x = p.y = 0.0f; break;
							default: p.x = p.y = 0.0f; break;
						}
						break;

					case MegaAxis.Y:
						switch ( eaxis )
						{
							case MegaAxis.X: p.y = p.z = 0.0f; break;
							case MegaAxis.Z: p.x = p.y = 0.0f; break;
							default: p.x = p.y = 0.0f; break;
						}
						break;

					case MegaAxis.Z:
						switch ( eaxis )
						{
							case MegaAxis.X: p.y = p.z = 0.0f; break;
							case MegaAxis.Y: p.x = p.z = 0.0f; break;
							default: p.y = p.z = 0.0f; break;
						}
						break;
				}
			}
			else
			{
				switch ( axis )
				{
					case MegaAxis.X: p.x = 0.0f; break;
					case MegaAxis.Y: p.y = 0.0f; break;
					case MegaAxis.Z: p.z = 0.0f; break;
				}
			}

			return p.normalized;
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			//float skewamount = Mathf.Atan(Mathf.Deg2Rad * angle);

			Vector3 skewv = GetSkew(p) * skewamount * p[(int)axis];

			p += skewv;

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool Prepare(MegaModContext mc)
		{
			skewamount = Mathf.Atan(Mathf.Deg2Rad * angle);
			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaRadialSkew mod = root as MegaRadialSkew;

			if ( mod )
			{
				angle		= mod.angle;
				axis		= mod.axis;
				eaxis		= mod.eaxis;
				biaxial		= mod.biaxial;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}