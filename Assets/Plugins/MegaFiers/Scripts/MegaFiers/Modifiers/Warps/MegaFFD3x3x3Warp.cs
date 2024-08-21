using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/FFD 3x3x3")]
	public class MegaFFD3x3x3Warp : MegaFFDWarp
	{
		Job			job;
		JobHandle	jobHandle;

		public override string WarpName() { return "FFD3x3x3"; }

		public override int GridSize()
		{
			return 3;
		}

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

				float sx = 1.0f - pp.x;
				float sy = 1.0f - pp.y;
				float sz = 1.0f - pp.z;

				for ( int i = 0; i < 3; i++ )
				{
					if ( i == 0 )
						ip = sx * sx;
					else
					{
						if ( i == 1 )
							ip = 2.0f * pp.x * sx;
						else
							ip = pp.x * pp.x;
					}

					for ( int j = 0; j < 3; j++ )
					{
						if ( j == 0 )
							jp = ip * sy * sy;
						else
						{
							if ( j == 1 )
								jp = ip * 2.0f * pp.y * sy;
							else
								jp = ip * pp.y * pp.y;
						}

						for ( int k = 0; k < 3; k++ )
						{
							if ( k == 0 )
								kp = jp * sz * sz;
							else
							{
								if ( k == 1 )
									kp = jp * 2.0f * pp.z * sz;
								else
									kp = jp * pp.z * pp.z;
							}

							int ix = (i * 9) + (j * 3) + k;
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

			float sx = 1.0f - pp.x;
			float sy = 1.0f - pp.y;
			float sz = 1.0f - pp.z;

			for ( int i = 0; i < 3; i++ )
			{
				if ( i == 0 )
					ip = sx * sx;
				else
				{
					if ( i == 1 )
						ip = 2.0f * pp.x * sx;
					else
						ip = pp.x * pp.x;
				}

				for ( int j = 0; j < 3; j++ )
				{
					if ( j == 0 )
						jp = ip * sy * sy;
					else
					{
						if ( j == 1 )
							jp = ip * 2.0f * pp.y * sy;
						else
							jp = ip * pp.y * pp.y;
					}

					for ( int k = 0; k < 3; k++ )
					{
						if ( k == 0 )
							kp = jp * sz * sz;
						else
						{
							if ( k == 1 )
								kp = jp * 2.0f * pp.z * sz;
							else
								kp = jp * pp.z * pp.z;
						}

						int ix = (i * 9) + (j * 3) + k;
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
			return (i * 9) + (j * 3) + k;
		}

		private void OnDestroy()
		{
			job.pt.Dispose();
		}
	}
}