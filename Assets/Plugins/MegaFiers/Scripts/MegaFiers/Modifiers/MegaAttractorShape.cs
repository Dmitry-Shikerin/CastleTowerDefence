using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	// 6.58ms old way 0.44ms new way
	public enum MegaAttractType
	{
		Attract,
		Repulse,
		Rotate,
	}

	[AddComponentMenu("Modifiers/Attractor Shape")]
	public class MegaAttractorShape : MegaModifier
	{
		public MegaShape		shape;
		public int				curve;
		public MegaAttractType	attractType		= MegaAttractType.Attract;
		[Adjust]
		public float			distance		= 0.0f;
		[Adjust]
		public float			rotate			= 0.0f;
		[Adjust]
		public float			force			= 0.0f;
		[Adjust]
		public float			slide			= 0.0f;
		public AnimationCurve	crv				= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		[Adjust]
		public int				itercount		= 4;
		int						k;
		Vector3					tangent;
		float					alpha;
		Vector3					delta;
		Vector3					nvp;
		Vector3					dir				= Vector3.zero;
		Matrix4x4				rottm			= Matrix4x4.identity;
		float					slidealpha;
		Matrix4x4				swtm;
		Matrix4x4				swltm;
		Matrix4x4				lwtm;
		Matrix4x4				wltm;
		public float			limit			= 1.0f;
		float					limit2			= 0.0f;
		Vector3					shapepos;
		public bool				flat			= true;
		public bool				splinechanged	= true;
		float					positiveInfinity;
		float					num2;
		public Vector3[]		points;
		Vector3					tp				= Vector3.zero;
		Vector3					qc				= Vector3.zero;
		Job						job;
		JobHandle				jobHandle;
		Matrix4x4				objectLocalToSplinetm;
		Matrix4x4				splineToObjectLocaltm;

		public override string ModName()	{ return "Attractor Shape"; }
		public override string GetHelpURL()	{ return "?page_id=338"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>		jvertices;
			public NativeArray<Vector3>		jsverts;
			public Matrix4x4				tm;
			public Matrix4x4				invtm;
			[Unity.Collections.ReadOnly]
			public megaspline				spline;
			public float3					shapepos;
			public float					limit2;
			public MegaAttractType			attractType;
			public int						itercount;
			public float					distance;
			public float					slide;
			public float					slidealpha;
			public float					force;
			public float					rotate;
			public bool						flat;
			public Matrix4x4				lwtm;
			public Matrix4x4				swtm;
			public Matrix4x4				swltm;
			public Matrix4x4				wltm;
			float3							delta;
			float3							nvp;
			float3							dir;
			float3							tp;
			Matrix4x4						rottm;
			public animationcurve			crv;

			public void Execute(int vi)
			{
				float alpha = 0.0f;
				int k = 0;

				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float3 vwp = lwtm.MultiplyPoint3x4(p);	// get vertex into spline space

				float3 qc = vwp - shapepos;

				if ( math.lengthsq(qc) < limit2 )
				{
					float3 splpos = spline.FindNearestPoint(swltm.MultiplyPoint3x4(vwp), itercount, ref alpha);
					splpos = swtm.MultiplyPoint3x4(splpos);

					if ( attractType == MegaAttractType.Repulse )
						delta = vwp - splpos;
					else
						delta = splpos - vwp;

					float len = math.length(delta);

					if ( len < distance )
					{
						float val = distance - len;
						float calpha = val / distance;
						float cval = crv.Evaluate(calpha);

						if ( attractType == MegaAttractType.Attract || attractType == MegaAttractType.Repulse )
						{
							float3 move = math.normalize(delta) * val * cval * force;

							if ( attractType == MegaAttractType.Attract )
							{
								if ( math.length(move) <= len )    // can used squared here?
									nvp = wltm.MultiplyPoint3x4(vwp + move);
								else
									nvp = wltm.MultiplyPoint3x4(splpos);
							}
							else
								nvp = wltm.MultiplyPoint3x4(vwp + move);
						}
						else
						{
							float alpha1;

							if ( slide >= 0.0f )
								alpha1 = alpha + ((1.0f - alpha) * slidealpha) * cval;
							else
								alpha1 = alpha + (alpha * slidealpha) * cval;

							if ( alpha1 < 0.0f )
								alpha1 = 0.0f;
							else
								if ( alpha1 >= 1.0f )
								alpha1 = 0.99999f;

							float3 fwd = swtm.MultiplyPoint3x4(spline.InterpCurve3D(alpha1, true, ref k));

							float alpha2 = alpha1 + 0.01f;

							if ( alpha1 + 0.01f >= 1.0f )
								alpha2 = alpha1 - 0.01f;

							float3 fwd1 = swtm.MultiplyPoint3x4(spline.InterpCurve3D(alpha2, true, ref k));

							if ( alpha + 0.01f < 1.0f )
								dir = math.normalize(fwd - fwd1);	//.normalized;
							else
								dir = math.normalize(fwd1 - fwd);	//.normalized;

							float3 rightVector = math.normalize(math.cross(delta, dir));	//      Vector3.Cross(delta, dir).normalized;

							//rottm.c0 = new float4(rightVector, 0);
							//rottm.c1 = new float4(math.cross(-rightVector, dir), 0);
							//rottm.c2 = new float4(dir, 0);
							//rottm.c3 = new float4(fwd, 0);

							rottm.SetColumn(0, new float4(rightVector, 0));
							rottm.SetColumn(1, new float4(math.cross(-rightVector, dir), 0));	//Vector3.Cross(-rightVector, dir));
							rottm.SetColumn(2, new float4(dir, 0));
							rottm.SetColumn(3, new float4(fwd, 0));

							float ag = (-90.0f + rotate * val * cval) * Mathf.Deg2Rad;
							tp.x = len * math.cos(ag);
							tp.y = len * math.sin(ag);
							tp.z = flat ? 0.0f : p.z;

							nvp = rottm.MultiplyPoint3x4(tp);
							nvp = wltm.MultiplyPoint3x4(nvp);
						}
					}
					else
						nvp = p;

					p = nvp;
				}

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.spline.points = new NativeArray<splinepoint>(shape.splines[curve].knots.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

				MegaSpline spline = shape.splines[curve];
				splinepoint sp;

				for ( int i = 0; i < spline.knots.Count; i++ )
				{
					sp.p			= spline.knots[i].p;
					sp.invec		= spline.knots[i].invec;
					sp.outvec		= spline.knots[i].outvec;
					sp.length		= spline.knots[i].length;
					sp.twist		= spline.knots[i].twist;
					sp.seglength	= spline.knots[i].seglength;

					job.spline.points[i] = sp;
				}

				job.spline.closed = spline.closed;
				job.spline.length = spline.length;
				job.spline.twistmode = spline.twistmode;

				job.swltm		= swltm;
				job.lwtm		= lwtm;
				job.swtm		= swtm;
				job.wltm		= wltm;
				job.rotate		= rotate;
				job.flat		= flat;
				job.force		= force;
				job.slide		= slide;
				job.slidealpha	= slidealpha;
				job.crv.keys	= new NativeArray<Keyframe>(crv.keys, Allocator.TempJob);
				job.distance	= distance;
				job.itercount	= itercount;
				job.attractType	= attractType;
				job.limit2		= limit2;
				job.shapepos	= shapepos;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();

				job.crv.keys.Dispose();
				job.spline.points.Dispose();    // only need to set once
			}
		}

		public Vector3 FindNearestPointWorld(Vector3 p, int iterations, ref float alpha)
		{
			return swtm.MultiplyPoint3x4(FindNearestPoint(swltm.MultiplyPoint3x4(p), iterations, ref alpha));
		}

		void Start()
		{
			PrepareShape();
		}

		void PrepareShape()
		{
			if ( points == null || points.Length == 0 )
				points = new Vector3[101];

			int kt = 0;

			int ix = 0;
			for ( float i = 0.0f; i <= 1.0f; i += 0.01f )
				points[ix++] = shape.splines[curve].Interpolate(i, true, ref kt);
		}

		void Find(Vector3 p)
		{
			positiveInfinity = float.PositiveInfinity;
			num2 = 0.0f;

			for ( int i = 0; i < 101; i++ )
			{
				float a = (float)i / 100.0f;
				Vector3 vector = points[i] - p;
				float sqrMagnitude = vector.sqrMagnitude;
				if ( positiveInfinity > sqrMagnitude )
				{
					positiveInfinity = sqrMagnitude;
					num2 = a;
				}
			}
		}

		// Find nearest point
		public Vector3 FindNearestPoint(Vector3 p, int iterations, ref float alpha)
		{
			int kt = 0;

			Find(p);
			MegaSpline spl = shape.splines[curve];
			for ( int j = 0; j < itercount; j++ )
			{
				float num6 = 0.01f * Mathf.Pow(10.0f, -((float)j));
				float num7 = num6 * 0.1f;
				for ( float k = Mathf.Clamp01(num2 - num6); k <= Mathf.Clamp01(num2 + num6); k += num7 )
				{
					Vector3 vector2 = spl.Interpolate(k, true, ref kt) - p;	
					float num9 = vector2.sqrMagnitude;

					if ( positiveInfinity > num9 )
					{
						positiveInfinity = num9;
						num2 = k;
					}
				}
			}

			alpha = num2;
			return shape.InterpCurve3D(curve, num2, true);
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			Vector3 vwp = lwtm.MultiplyPoint3x4(p);

			qc.x = vwp.x - shapepos.x;
			qc.y = vwp.y - shapepos.y;
			qc.z = vwp.z - shapepos.z;

			if ( qc.sqrMagnitude < limit2 )
			{
				Vector3 splpos = FindNearestPointWorld(vwp, itercount, ref alpha);
				
				if ( attractType == MegaAttractType.Repulse )
				{
					delta.x = vwp.x - splpos.x;
					delta.y = vwp.y - splpos.y;
					delta.z = vwp.z - splpos.z;
				}
				else
				{
					delta.x = splpos.x - vwp.x;
					delta.y = splpos.y - vwp.y;
					delta.z = splpos.z - vwp.z;
				}

				float len = delta.magnitude;
				
				if ( len  < distance )
				{
					float val = distance - len;
					float calpha = val / distance;
					float cval = crv.Evaluate(calpha);
							
					if ( attractType == MegaAttractType.Attract || attractType == MegaAttractType.Repulse )
					{
						Vector3 move = delta.normalized * val * cval * force;
				
						if ( attractType == MegaAttractType.Attract )
						{					
							if ( move.magnitude <= len )	// can used squared here?
								nvp = wltm.MultiplyPoint3x4(vwp + move);
							else
								nvp = wltm.MultiplyPoint3x4(splpos);
						}
						else
							nvp = wltm.MultiplyPoint3x4(vwp + move);
					}
					else
					{
						float alpha1;

						if ( slide >= 0.0f )
							alpha1 = alpha + ((1.0f - alpha) * slidealpha) * cval;
						else
							alpha1 = alpha + (alpha * slidealpha) * cval;
				
						if ( alpha1 < 0.0f )
							alpha1 = 0.0f;
						else
							if ( alpha1 >= 1.0f )
								alpha1 = 0.99999f;
							
						Vector3 fwd = swtm.MultiplyPoint3x4(shape.splines[curve].InterpCurve3D(alpha1, true, ref k));
						
						float alpha2 = alpha1 + 0.01f;
				
						if ( alpha1 + 0.01f >= 1.0f )
							alpha2 = alpha1 - 0.01f;

						Vector3 fwd1 = swtm.MultiplyPoint3x4(shape.splines[curve].InterpCurve3D(alpha2, true, ref k));
						
						if ( alpha + 0.01f < 1.0f )
							dir = (fwd - fwd1).normalized;
						else
							dir = (fwd1 - fwd).normalized;
						
						Vector3 rightVector = Vector3.Cross(delta, dir).normalized;

						rottm.SetColumn(0, rightVector);
						rottm.SetColumn(1, Vector3.Cross(-rightVector, dir));
						rottm.SetColumn(2, dir);
						rottm.SetColumn(3, fwd);
	
						float ag = (-90.0f + rotate * val * cval) * Mathf.Deg2Rad;
						tp.x = len * Mathf.Cos(ag);
						tp.y = len * Mathf.Sin(ag);
						tp.z = flat ? 0.0f : p.z;
						nvp = rottm.MultiplyPoint3x4(tp);
						nvp = wltm.MultiplyPoint3x4(nvp);
					}
				}
				else
					nvp = p;
	
				p = nvp;
			}

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( shape )
			{
				if ( splinechanged || points == null || points.Length == 0 )
				{
					PrepareShape();
					splinechanged = false;
				}

				limit2 = limit * limit;
				shapepos = shape.transform.position;
				slidealpha = slide * 0.01f;
				swtm = shape.transform.localToWorldMatrix;
				swltm = shape.transform.worldToLocalMatrix;
				lwtm = transform.localToWorldMatrix;
				wltm = transform.worldToLocalMatrix;

				objectLocalToSplinetm = transform.localToWorldMatrix * shape.transform.worldToLocalMatrix;
				splineToObjectLocaltm = shape.transform.localToWorldMatrix * transform.worldToLocalMatrix;

				return true;
			}

			return false;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaAttractorShape mod = root as MegaAttractorShape;

			if ( mod )
			{
				shape			= mod.shape;
				curve			= mod.curve;
				attractType		= mod.attractType;
				distance		= mod.distance;
				rotate			= mod.rotate;
				force			= mod.force;
				slide			= mod.slide;
				crv.keys		= mod.crv.keys;
				itercount		= mod.itercount;
				limit			= mod.limit;
				flat			= mod.flat;
				gizmoPos		= mod.gizmoPos;
				gizmoRot		= mod.gizmoRot;
				gizmoScale		= mod.gizmoScale;
				bbox			= mod.bbox;
			}
		}
	}
}