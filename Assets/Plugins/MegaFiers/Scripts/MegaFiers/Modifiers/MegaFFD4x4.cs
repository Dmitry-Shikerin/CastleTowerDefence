using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/FFD/2D/FFD 4x4")]
	public class MegaFFD4x4 : MegaFFD
	{
		Job			job;
		JobHandle	jobHandle;

		public override string ModName() { return "FFD4x4"; }

		public override int GridSize()
		{
			return 4;
		}

		public override int NumPoints() { return 4 * 4; }

		public override void Dispose()
		{
			if ( job.pt.IsCreated )
				job.pt.Dispose();
		}

		public override bool AnyDeform(float epsilon = 0.0001f)
		{
			int size = GridSize();
			float fsize = size - 1.0f;

			for ( int i = 0; i < size; i++ )
			{
				float isz = (float)(i) / fsize;

				for ( int j = 0; j < size; j++ )
				{
					float jsz = (float)(j) / fsize;

					int c = GridIndex(i, j, 0);
					if ( Mathf.Abs(pt[c].x - isz) > epsilon )
						return true;
					if ( Mathf.Abs(pt[c].y - jsz) > epsilon )
						return true;
				}
			}

			return false;
		}

		public override bool Changed()
		{
			if ( ModEnabled )
			{
				lastEnabled = ModEnabled;
				if ( alwaysChanging )
					return true;

				int size = GridSize();
				for ( int i = 0; i < size; i++ )
				{
					for ( int j = 0; j < size; j++ )
					{
						int c = GridIndex(i, j, 0);

						if ( pt[c] != lastpt[c] )
						{
							lastpt[c] = pt[c];
							return true;
						}
					}
				}
			}
			else
			{
				if ( ModEnabled != lastEnabled )
				{
					lastEnabled = ModEnabled;
					lastpt[0] = pt[0] + Vector3.up;
					return true;
				}
			}

			return false;
		}

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public bool					invol;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3> pt;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public float				EPSILON;

			public void Execute(int vi)
			{
				Vector3 q = Vector3.zero;
				Vector3 pp = tm.MultiplyPoint3x4(jvertices[vi]);

				if ( invol )
				{
					for ( int i = 0; i < 3; i++ )
					{
						if ( pp[i] < -EPSILON || pp[i] > 1.0f + EPSILON )
						{
							jsverts[vi] = jvertices[vi];
							return;
						}
					}
				}

				float ip, jp, kp;

				float sx = 1.0f - pp.x;
				float sy = 1.0f - pp.y;
				float sz = 1.0f - pp.z;
				float sy2 = sy * sy;
				float py = pp.y * pp.y;
				float sz2 = sz * sz;
				float pz = pp.z * pp.z;

				for ( int i = 0; i < 4; i++ )
				{
					if ( i == 0 )
						ip = (sx * sx * sx);
					else
					{
						if ( i == 1 )
							ip = (3.0f * pp.x * sx * sx);
						else
						{
							if ( i == 2 )
								ip = (3.0f * pp.x * pp.x * sx);
							else
								ip = (pp.x * pp.x * pp.x);
						}
					}

					for ( int j = 0; j < 4; j++ )
					{
						if ( j == 0 )
							jp = ip * (sy * sy2);
						else
						{
							if ( j == 1 )
								jp = ip * (3.0f * pp.y * sy2);
							else
							{
								if ( j == 2 )
									jp = ip * (3.0f * py * sy);
								else
									jp = ip * (py * pp.y);
							}
						}

						kp = jp * (sz * sz2);

						int ix = (i << 4) + (j << 2);
						q += pt[ix] * kp;
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

			float ip, jp, kp;

			float sx = 1.0f - pp.x;
			float sy = 1.0f - pp.y;
			float sz = 1.0f - pp.z;
			float sy2 = sy * sy;
			float py = pp.y * pp.y;
			float sz2 = sz * sz;
			float pz = pp.z * pp.z;

			for ( int i = 0; i < 4; i++ )
			{
				if ( i == 0 )
					ip = (sx * sx * sx);
				else
				{
					if ( i == 1 )
						ip = (3.0f * pp.x * sx * sx);
					else
					{
						if ( i == 2 )
							ip = (3.0f * pp.x * pp.x * sx);
						else
							ip = (pp.x * pp.x * pp.x);
					}
				}

				for ( int j = 0; j < 4; j++ )
				{
					if ( j == 0 )
						jp = ip * (sy * sy2);
					else
					{
						if ( j == 1 )
							jp = ip * (3.0f * pp.y * sy2);
						else
						{
							if ( j == 2 )
								jp = ip * (3.0f * py * sy);
							else
								jp = ip * (py * pp.y);
						}
					}

					kp = jp * (sz * sz2);
					int ix = (i << 4) + (j << 2);
					q.x += pt[ix].x * kp;
					q.y += pt[ix].y * kp;
					q.z += pt[ix].z * kp;
				}
			}

			return invtm.MultiplyPoint3x4(q);
		}

		public override int GridIndex(int i, int j, int k)
		{
			return (i << 4) + (j << 2);
		}

		public override void GridXYZ(int index, out int x, out int y, out int z)
		{
			x = (index / 16) % 4;
			y = (index / 4) % 4;
			z = 0;
		}
	}
}