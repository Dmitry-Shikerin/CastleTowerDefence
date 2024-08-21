using UnityEngine;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	public enum MegaAlter
	{
		Offset,
		Scale,
		Both,
	}

	public enum MegaAffect
	{
		X,
		Y,
		Z,
		XY,
		XZ,
		YZ,
		XYZ,
		None,
	}

	[System.Serializable]
	public class MegaSculptCurve
	{
		public MegaSculptCurve()
		{
			curve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 0.0f));

			offamount		= Vector3.one;
			sclamount		= Vector3.one;
			axis			= MegaAxis.X;
			affectOffset	= MegaAffect.Y;
			affectScale		= MegaAffect.None;
			enabled			= true;
			weight			= 1.0f;
			name			= "None";
			uselimits		= false;
		}

		public AnimationCurve	curve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 0.0f));

		public Vector3			offamount		= Vector3.one;
		public Vector3			sclamount		= Vector3.one;
		public MegaAxis			axis			= MegaAxis.X;
		public MegaAffect		affectOffset	= MegaAffect.Y;
		public MegaAffect		affectScale		= MegaAffect.None;
		public bool				enabled			= true;
		public float			weight			= 1.0f;
		public string			name			= "None";
		public Color			regcol			= Color.yellow;
		public Vector3			origin			= Vector3.zero;
		public Vector3			boxsize			= Vector3.one;
		public bool				uselimits		= false;
		public Vector3			size			= Vector3.zero;

		static public MegaSculptCurve Create()
		{
			MegaSculptCurve crv = new MegaSculptCurve();
			return crv;
		}
	}

#if true
#else
	struct sculptcurve
	{
		public float3					offamount;
		public float3					sclamount;
		public MegaAxis					axis;
		public MegaAffect				affectOffset;
		public MegaAffect				affectScale;
		public bool						enabled;
		public float					weight;
		public float3					origin;
		public float3					boxsize;
		public bool						uselimits;
		public float3					size;
	}
#endif

	[AddComponentMenu("Modifiers/Curve Sculpt Layered")]
	public class MegaCurveSculptLayered : MegaModifier
	{
		public List<MegaSculptCurve>	curves	= new List<MegaSculptCurve>();
		Vector3							size	= Vector3.zero;
		public override string ModName()	{ return "CurveSculpLayered"; }
		public override string GetHelpURL() { return "?page_id=2411"; }

#if true
#else
		Job								job;
		JobHandle						jobHandle;

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float3						min;
			public float3						size;
			[Unity.Collections.ReadOnly]
			public NativeArray<sculptcurve>		curves;
			[Unity.Collections.ReadOnly]
			public animationcurve				animcurve0;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>			jvertices;
			public NativeArray<Vector3>			jsverts;
			public Matrix4x4					tm;
			public Matrix4x4					invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				for ( int c = 0; c < curves.Length; c++ )
				{
					sculptcurve crv = curves[c];

					if ( crv.enabled )
					{
						int ax = (int)crv.axis;

						if ( crv.uselimits )
						{
							// Is the point in the box
							float3 bp = p - crv.origin;
							if ( math.abs(bp.x) < crv.size.x && math.abs(bp.y) < crv.size.y && math.abs(bp.z) < crv.size.z )
							{
								float alpha = 0.5f + ((bp[ax] / crv.size[ax]) * 0.5f);

								if ( alpha >= 0.0f && alpha <= 1.0f )
								{
									float a = 1.0f * crv.weight;    //animcurve0.Evaluate(alpha) * crv.weight;

									switch ( crv.affectScale )
									{
										case MegaAffect.X:
											p.x += bp.x * (a * crv.sclamount.x);
											break;

										case MegaAffect.Y:
											p.y += bp.y * (a * crv.sclamount.y);
											break;

										case MegaAffect.Z:
											p.z += bp.z * (a * crv.sclamount.z);
											break;

										case MegaAffect.XY:
											p.x += bp.x * (a * crv.sclamount.x);
											p.y += bp.y * (a * crv.sclamount.y);
											break;

										case MegaAffect.XZ:
											p.x += bp.x * (a * crv.sclamount.x);
											p.z += bp.z * (a * crv.sclamount.z);
											break;

										case MegaAffect.YZ:
											p.y += bp.y * (a * crv.sclamount.y);
											p.z += bp.z * (a * crv.sclamount.z);
											break;

										case MegaAffect.XYZ:
											p.x += bp.x * (a * crv.sclamount.x);
											p.y += bp.y * (a * crv.sclamount.y);
											p.z += bp.z * (a * crv.sclamount.z);
											break;
									}

									switch ( crv.affectOffset )
									{
										case MegaAffect.X:
											p.x += a * crv.offamount.x;
											break;

										case MegaAffect.Y:
											p.y += a * crv.offamount.y;
											break;

										case MegaAffect.Z:
											p.z += a * crv.offamount.z;
											break;

										case MegaAffect.XY:
											p.x += a * crv.offamount.x;
											p.y += a * crv.offamount.y;
											break;

										case MegaAffect.XZ:
											p.x += a * crv.offamount.x;
											p.z += a * crv.offamount.z;
											break;

										case MegaAffect.YZ:
											p.y += a * crv.offamount.y;
											p.z += a * crv.offamount.z;
											break;

										case MegaAffect.XYZ:
											p.x += a * crv.offamount.x;
											p.y += a * crv.offamount.y;
											p.z += a * crv.offamount.z;
											break;
									}
								}
							}
						}
						else
						{
							float alpha = (p[ax] - min[ax]) / size[ax];
							float a = 1.0f * crv.weight;	//animcurve0.Evaluate(alpha) * crv.weight;

							switch ( crv.affectScale )
							{
								case MegaAffect.X:
									p.x *= 1.0f + (a * crv.sclamount.y);
									break;

								case MegaAffect.Y:
									p.y *= 1.0f + (a * crv.sclamount.y);
									break;

								case MegaAffect.Z:
									p.z *= 1.0f + (a * crv.sclamount.z);
									break;

								case MegaAffect.XY:
									p.x *= 1.0f + (a * crv.sclamount.y);
									p.y *= 1.0f + (a * crv.sclamount.y);
									break;

								case MegaAffect.XZ:
									p.x *= 1.0f + (a * crv.sclamount.y);
									p.z *= 1.0f + (a * crv.sclamount.z);
									break;

								case MegaAffect.YZ:
									p.y *= 1.0f + (a * crv.sclamount.y);
									p.z *= 1.0f + (a * crv.sclamount.z);
									break;

								case MegaAffect.XYZ:
									p.x *= 1.0f + (a * crv.sclamount.y);
									p.y *= 1.0f + (a * crv.sclamount.y);
									p.z *= 1.0f + (a * crv.sclamount.z);
									break;
							}

							switch ( crv.affectOffset )
							{
								case MegaAffect.X:
									p.x += a * crv.offamount.x;
									break;

								case MegaAffect.Y:
									p.y += a * crv.offamount.y;
									break;

								case MegaAffect.Z:
									p.z += a * crv.offamount.z;
									break;

								case MegaAffect.XY:
									p.x += a * crv.offamount.x;
									p.y += a * crv.offamount.y;
									break;

								case MegaAffect.XZ:
									p.x += a * crv.offamount.x;
									p.z += a * crv.offamount.z;
									break;

								case MegaAffect.YZ:
									p.y += a * crv.offamount.y;
									p.z += a * crv.offamount.z;
									break;

								case MegaAffect.XYZ:
									p.x += a * crv.offamount.x;
									p.y += a * crv.offamount.y;
									p.z += a * crv.offamount.z;
									break;
							}
						}
					}
				}

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}
#endif

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
#if true
				for ( int i = 0; i < jverts.Length; i++ )
				{
					jsverts[i] = Map(i, jverts[i]);
				}
#else
				if ( curves.Count > 0 )
				{
					job.curves = new NativeArray<sculptcurve>(curves.Count, Allocator.TempJob);	//, NativeArrayOptions.UninitializedMemory);

					for ( int i = 0; i < curves.Count; i++ )
					{
						job.animcurve0.keys = new NativeArray<Keyframe>(curves[i].curve.keys, Allocator.TempJob);

						//Debug.Log("keys " + job.animcurve0.keys.Length);
						sculptcurve c = job.curves[i];
						c.offamount		= curves[i].offamount;
						c.sclamount		= curves[i].sclamount;
						c.axis			= curves[i].axis;
						//Debug.Log("axis " + (int)c.axis);
						c.affectOffset	= curves[i].affectOffset;
						c.affectScale	= curves[i].affectScale;
						c.enabled		= curves[i].enabled;
						Debug.Log("c enabled " + curves[i].enabled + " cen " + c.enabled + " i " + i);

						c.weight		= curves[i].weight;
						c.origin		= curves[i].origin;
						c.boxsize		= curves[i].boxsize;
						c.uselimits		= curves[i].uselimits;
						c.size			= curves[i].size;
					}

					job.min			= bbox.min;
					job.size		= size;
					job.jvertices	= jverts;
					job.jsverts		= jsverts;
					job.tm			= tm;
					job.invtm		= invtm;

					jobHandle = job.Schedule(jverts.Length, mc.batchCount);
					jobHandle.Complete();

					//for ( int i = 0; i < job.animcurves.Length; i++ )
					{
						job.animcurve0.keys.Dispose();
					}

					job.curves.Dispose();
					//job.animcurves.Dispose();
				}
#endif
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			for ( int c = 0; c < curves.Count; c++ )
			{
				MegaSculptCurve crv = curves[c];

				if ( crv.enabled )
				{
					int ax = (int)crv.axis;

					if ( crv.uselimits )
					{
						// Is the point in the box
						Vector3 bp = p - crv.origin;
						if ( Mathf.Abs(bp.x) < crv.size.x && Mathf.Abs(bp.y) < crv.size.y && Mathf.Abs(bp.z) < crv.size.z )
						{
							float alpha = 0.5f + ((bp[ax] / crv.size[ax]) * 0.5f);

							if ( alpha >= 0.0f && alpha <= 1.0f )
							{
								float a = crv.curve.Evaluate(alpha) * crv.weight;

								switch ( crv.affectScale )
								{
									case MegaAffect.X:
										p.x += bp.x * (a * crv.sclamount.x);
										break;

									case MegaAffect.Y:
										p.y += bp.y * (a * crv.sclamount.y);
										break;

									case MegaAffect.Z:
										p.z += bp.z * (a * crv.sclamount.z);
										break;

									case MegaAffect.XY:
										p.x += bp.x * (a * crv.sclamount.x);
										p.y += bp.y * (a * crv.sclamount.y);
										break;

									case MegaAffect.XZ:
										p.x += bp.x * (a * crv.sclamount.x);
										p.z += bp.z * (a * crv.sclamount.z);
										break;

									case MegaAffect.YZ:
										p.y += bp.y * (a * crv.sclamount.y);
										p.z += bp.z * (a * crv.sclamount.z);
										break;

									case MegaAffect.XYZ:
										p.x += bp.x * (a * crv.sclamount.x);
										p.y += bp.y * (a * crv.sclamount.y);
										p.z += bp.z * (a * crv.sclamount.z);
										break;
								}

								switch ( crv.affectOffset )
								{
									case MegaAffect.X:
										p.x += a * crv.offamount.x;
										break;

									case MegaAffect.Y:
										p.y += a * crv.offamount.y;
										break;

									case MegaAffect.Z:
										p.z += a * crv.offamount.z;
										break;

									case MegaAffect.XY:
										p.x += a * crv.offamount.x;
										p.y += a * crv.offamount.y;
										break;

									case MegaAffect.XZ:
										p.x += a * crv.offamount.x;
										p.z += a * crv.offamount.z;
										break;

									case MegaAffect.YZ:
										p.y += a * crv.offamount.y;
										p.z += a * crv.offamount.z;
										break;

									case MegaAffect.XYZ:
										p.x += a * crv.offamount.x;
										p.y += a * crv.offamount.y;
										p.z += a * crv.offamount.z;
										break;
								}
							}
						}
					}
					else
					{
						float alpha = (p[ax] - bbox.min[ax]) / size[ax];
						float a = crv.curve.Evaluate(alpha) * crv.weight;

						switch ( crv.affectScale )
						{
							case MegaAffect.X:
								p.x *= 1.0f + (a * crv.sclamount.y);
								break;

							case MegaAffect.Y:
								p.y *= 1.0f + (a * crv.sclamount.y);
								break;

							case MegaAffect.Z:
								p.z *= 1.0f + (a * crv.sclamount.z);
								break;

							case MegaAffect.XY:
								p.x *= 1.0f + (a * crv.sclamount.y);
								p.y *= 1.0f + (a * crv.sclamount.y);
								break;

							case MegaAffect.XZ:
								p.x *= 1.0f + (a * crv.sclamount.y);
								p.z *= 1.0f + (a * crv.sclamount.z);
								break;

							case MegaAffect.YZ:
								p.y *= 1.0f + (a * crv.sclamount.y);
								p.z *= 1.0f + (a * crv.sclamount.z);
								break;

							case MegaAffect.XYZ:
								p.x *= 1.0f + (a * crv.sclamount.y);
								p.y *= 1.0f + (a * crv.sclamount.y);
								p.z *= 1.0f + (a * crv.sclamount.z);
								break;
						}

						switch ( crv.affectOffset )
						{
							case MegaAffect.X:
								p.x += a * crv.offamount.x;
								break;

							case MegaAffect.Y:
								p.y += a * crv.offamount.y;
								break;

							case MegaAffect.Z:
								p.z += a * crv.offamount.z;
								break;

							case MegaAffect.XY:
								p.x += a * crv.offamount.x;
								p.y += a * crv.offamount.y;
								break;

							case MegaAffect.XZ:
								p.x += a * crv.offamount.x;
								p.z += a * crv.offamount.z;
								break;

							case MegaAffect.YZ:
								p.y += a * crv.offamount.y;
								p.z += a * crv.offamount.z;
								break;

							case MegaAffect.XYZ:
								p.x += a * crv.offamount.x;
								p.y += a * crv.offamount.y;
								p.z += a * crv.offamount.z;
								break;
						}
					}
				}
			}

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			size = bbox.max - bbox.min;

			for ( int i = 0; i < curves.Count; i++ )
			{
				curves[i].size = curves[i].boxsize * 0.5f;
			}
			return true;
		}

		public override void DrawGizmo(MegaModContext context)
		{
			base.DrawGizmo(context);

			for ( int i = 0; i < curves.Count; i++ )
			{
				if ( curves[i].enabled && curves[i].uselimits )
				{
					Gizmos.color = curves[i].regcol;
					Gizmos.DrawWireCube(curves[i].origin, curves[i].boxsize);
				}
			}
		}
	}
}