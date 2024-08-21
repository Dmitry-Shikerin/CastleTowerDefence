using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Hit")]
	public class MegaHitDeform : MegaModifier
	{
		public override string		ModName()		{ return "Hit Deform"; }
		[Adjust]
		public float				hitradius		= 1.0f;
		[Adjust]
		public float				Hardness		= 0.5f;
		[Adjust]
		public float				deformlimit		= 0.1f;
		[Adjust]
		public float				scaleforce		= 1000.0f;
		[Adjust]
		public float				maxForce		= 1.0f;
		NativeArray<Vector3>		offsets;
		float						msize;
		Job							job;
		JobHandle					jobHandle;

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public NativeArray<Vector3> offsets;

			public void Execute(int i)
			{
				jsverts[i] = jvertices[i] + offsets[i];
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.jvertices	= jverts;
				job.jsverts		= jsverts;
				job.offsets		= offsets;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public void Deform(Vector3 point, Vector3 normal, float force)
		{
			force = Mathf.Min(maxForce, force);

			if ( force > 0.01f )
			{
				float df = force * (msize * (0.1f / Mathf.Max(0.1f, Hardness)));

				float max = deformlimit;
				float maxsqr = max * max;

				Vector3 p = transform.InverseTransformPoint(point);
				Vector3 nr = transform.InverseTransformDirection(normal);

				for ( int i = 0; i < verts.Length; i++ )
				{
					float d = ((verts[i] + offsets[i]) - p).sqrMagnitude;
					if ( d <= df )
					{
						Vector3 n = nr * (1.0f - (d / df)) * df;

						offsets[i] += n;

						if ( deformlimit > 0.0f )
						{
							n = offsets[i];
							d = n.sqrMagnitude;
							if ( d > maxsqr )
								offsets[i] = (n * (max / n.magnitude));
						}
					}
				}
			}
		}

		void Deform(Collision collision)
		{
			float cf = Mathf.Min(maxForce, collision.relativeVelocity.sqrMagnitude / scaleforce);
     
			if ( cf > 0.01f )
			{
				ContactPoint[] contacts = collision.contacts;

				for ( int c = 0; c < contacts.Length; c++ )
					Deform(contacts[c].point, contacts[c].normal, cf);
			}
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( offsets == null || offsets.Length != mc.mod.jverts.Length )
			{
				//offsets = new NativeArray<Vector3>(mc.mod.jverts.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				offsets = new NativeArray<Vector3>(mc.mod.jverts.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}

			msize = MegaUtils.SmallestValue(mc.bbox.Size());
			return true;
		}

		public void Repair(float repair)
		{
			Repair(repair, Vector3.zero, 0.0f);
		}

		public void Repair(float repair, Vector3 point, float radius)
		{
			point = transform.InverseTransformPoint(point);

			float rsqr = radius * radius;
			for ( int i = 0; i < offsets.Length; i++ )
			{
				if ( radius > 0.0f )
				{
					Vector3 vector3 = point - verts[i];
					if ( vector3.sqrMagnitude >= rsqr )
						break;
				}
				offsets[i] *= repair;
			}
		}

		public void OnCollisionEnter(Collision collision)
		{
			Deform(collision);
		}

		public void OnCollisionStay(Collision collision)
		{
			Deform(collision);
		}

		public void OnDestroy()
		{
			offsets.Dispose();
		}
	}
}