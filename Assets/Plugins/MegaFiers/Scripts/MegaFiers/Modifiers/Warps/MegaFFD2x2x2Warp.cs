using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/FFD 2x2x2")]
	public class MegaFFD2x2x2Warp : MegaFFDWarp
	{
		Job			job;
		JobHandle	jobHandle;

		public override string WarpName() { return "FFD2x2x2"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public bool					inVol;
			public float				EPSILON;
			public NativeArray<Vector3>	pt;
			public float				totaldecay;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			wtm;
			public Matrix4x4			winvtm;

			public void Execute(int vi)
			{
				float3 p = wtm.MultiplyPoint3x4(jvertices[vi]);

				Vector3 pp = tm.MultiplyPoint3x4(p);

				if ( inVol )
				{
					for ( int i = 0; i < 3; i++ )
					{
						if ( pp[i] < -EPSILON || pp[i] > 1.0f + EPSILON )
						{
							jsverts[vi] = winvtm.MultiplyPoint3x4(p);
							return;
						}
					}
				}

				Vector3 q = Vector3.zero;

				Vector3 ipp = pp;
				float dist = pp.magnitude;
				float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

				float ip, jp, kp;
				for ( int i = 0; i < 2; i++ )
				{
					ip = i == 0 ? 1.0f - pp.x : pp.x;

					for ( int j = 0; j < 2; j++ )
					{
						jp = ip * (j == 0 ? 1.0f - pp.y : pp.y);

						for ( int k = 0; k < 2; k++ )
						{
							kp = jp * (k == 0 ? 1.0f - pp.z : pp.z);

							int ix = (i * 4) + (j * 2) + k;
							q.x += pt[ix].x * kp;
							q.y += pt[ix].y * kp;
							q.z += pt[ix].z * kp;
						}
					}
				}

				q = math.lerp(ipp, q, dcy);

				p = invtm.MultiplyPoint3x4(q);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				if ( job.pt.Length == 0 )
					job.pt = new NativeArray<Vector3>(pt, Allocator.Persistent);
				else
				{
					for ( int i = 0; i < pt.Length; i++ )
						job.pt[i] = pt[i];
				}

				job.inVol		= inVol;
				job.EPSILON		= EPSILON;
				job.totaldecay	= totaldecay;
				job.tm			= tm;
				job.invtm		= invtm;
				job.wtm			= mod.tm;
				job.winvtm		= mod.invtm;
				job.jvertices	= mod.jverts;
				job.jsverts		= mod.jsverts;

				jobHandle = job.Schedule(mod.jverts.Length, 64);
				jobHandle.Complete();
			}
		}

		public override int GridSize()
		{
			return 2;
		}

		public override Vector3 Map(int ii, Vector3 p)
		{
			Vector3 q = Vector3.zero;

			Vector3 pp = tm.MultiplyPoint3x4(p);

			if ( inVol )
			{
				for ( int i = 0; i < 3; i++ )
				{
					if ( pp[i] < -EPSILON || pp[i] > 1.0f + EPSILON )
						return p;
				}
			}

			Vector3 ipp = pp;
			float dist = pp.magnitude;
			float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

			float ip, jp, kp;
			for ( int i = 0; i < 2; i++ )
			{
				ip = i == 0 ? 1.0f - pp.x : pp.x;

				for ( int j = 0; j < 2; j++ )
				{
					jp = ip * (j == 0 ? 1.0f - pp.y : pp.y);

					for ( int k = 0; k < 2; k++ )
					{
						kp = jp * (k == 0 ? 1.0f - pp.z : pp.z);

						int ix = (i * 4) + (j * 2) + k;
						q.x += pt[ix].x * kp;
						q.y += pt[ix].y * kp;
						q.z += pt[ix].z * kp;
					}
				}
			}

			q = Vector3.Lerp(ipp, q, dcy);

			return invtm.MultiplyPoint3x4(q);
		}

		public override int GridIndex(int i, int j, int k)
		{
			return (i * 4) + (j * 2) + k;
		}

		private void OnDestroy()
		{
			job.pt.Dispose();
		}
	}
}