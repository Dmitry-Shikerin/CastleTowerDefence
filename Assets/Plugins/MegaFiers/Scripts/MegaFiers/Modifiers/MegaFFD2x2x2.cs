using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/FFD/3D/FFD 2x2x2")]
	public class MegaFFD2x2x2 : MegaFFD
	{
		Job			job;
		JobHandle	jobHandle;

		public override string ModName() { return "FFD2x2x2"; }

		public override int GridSize()
		{
			return 2;
		}

		public override void Dispose()
		{
			if ( job.pt.IsCreated )
				job.pt.Dispose();
		}

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public bool					invol;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	pt;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public float				EPSILON;

			public void Execute(int vi)
			{
				Vector3 q = Vector3.zero;

				Vector3 pp = tm.MultiplyPoint3x4(jvertices[vi]);

				if ( invol )
				{
					for ( int j = 0; j < 3; j++ )
					{
						if ( pp[j] < -EPSILON || pp[j] > 1.0f + EPSILON )
						{
							jsverts[vi] = jvertices[vi];
							return;
						}
					}
				}

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

				jsverts[vi] = invtm.MultiplyPoint3x4(q);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				if ( job.pt.Length == 0 )
					job.pt = new NativeArray<Vector3>(pt, Allocator.Persistent);
				else
				{
					for ( int i = 0; i < pt.Length; i++ )
						job.pt[i] = pt[i];
				}

				job.EPSILON		= EPSILON;
				job.invol		= inVol;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
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

			float ip,jp,kp;
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

			return invtm.MultiplyPoint3x4(q);
		}

		public override int GridIndex(int i, int j, int k)
		{
			return (i * 4) + (j * 2) + k;
		}

		public override void GridXYZ(int index, out int x, out int y, out int z)
		{
			x = (index / 4) % 2;
			y = (index / 2) % 2;
			z = index % 2;
		}
	}
}