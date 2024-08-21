using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	public enum MegaNormType
	{
		Normals = 0,
		Vertices,
		Average,
		NotSet,
	}

	[AddComponentMenu("Modifiers/Push")]
	public class MegaPush : MegaModifier
	{
		[Adjust]
		public float				amount		= 0.0f;
		public MegaNormType			method		= MegaNormType.Normals;
		public MegaNormType			lastMethod	= MegaNormType.NotSet;
		[Unity.Collections.ReadOnly]
		public NativeArray<Vector3>	normals;
		Job							job;
		JobHandle					jobHandle;

		public override string ModName() { return "Push"; }
		public override string GetHelpURL() { return "?page_id=282"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				amount;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3> normals;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;

			public void Execute(int vi)
			{
				jsverts[vi] = jvertices[vi] + (normals[vi] * amount);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.amount		= amount;
				job.normals		= normals;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			if ( i >= 0 )
				p += normals[i] * amount;
			else
				p += Vector3.up * amount * 0.1f;

			return p;
		}

		public override void Dispose()
		{
			if ( normals.IsCreated )
				normals.Dispose();
		}

		void CalcNormals(Mesh mesh, MegaModifyObject obj)
		{
			if ( mesh != null )
			{
				if ( normals.IsCreated )
					normals.Dispose();

				switch ( method )
				{
					case MegaNormType.Normals:
						normals = new NativeArray<Vector3>(mesh.normals, Allocator.Persistent);
						break;

					case MegaNormType.Vertices:
						normals = new NativeArray<Vector3>(mesh.normals.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

						for ( int i = 0; i < obj.startverts.Length; i++ )
							normals[i] = Vector3.Normalize(obj.startverts[i]);
						break;

					case MegaNormType.Average:
						normals = new NativeArray<Vector3>(mesh.normals, Allocator.Persistent);

						int count = obj.startverts.Length;

						for ( int i = 0; i < count; i++ )
						{
							for ( int j = 0; j < count; j++ )
							{
								if ( obj.startverts[i] == obj.startverts[j] )
								{
									normals[i] = (normals[i] + normals[j]) / 2.0f;
									normals[j] = (normals[i] + normals[j]) / 2.0f;
								}
							}
						}
						break;
				}
			}
		}

		public override void ModStart(MegaModifyObject mc)
		{
			CalcNormals(mc.mesh, mc);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( method != lastMethod )
			{
				CalcNormals(mc.mod.mesh, mc.mod);
				lastMethod = method;
			}

			if ( normals.IsCreated )
				return true;

			CalcNormals(mc.mod.mesh, mc.mod);
			lastMethod = method;

			return true;
		}

		void Reset()
		{
			Renderer rend = GetComponent<Renderer>();

			if ( rend != null )
			{
				Mesh ms = MegaUtils.GetSharedMesh(gameObject);

				if ( ms != null )
				{
					CalcNormals(ms, GetComponent<MegaModifyObject>());

					Bounds b = ms.bounds;
					Offset = -b.center;
					bbox.min = b.center - b.extents;
					bbox.max = b.center + b.extents;
				}
			}
		}

		private void OnDestroy()
		{
			if ( normals.IsCreated )
				normals.Dispose();
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaPush mod = root as MegaPush;

			if ( mod )
			{
				amount		= mod.amount;
				method		= mod.method;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}