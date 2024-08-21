using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Scale")]
	public class MegaScale : MegaModifier
	{
		[Adjust]
		public Vector3	scale = Vector3.one;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Scale"; }
		public override string GetHelpURL() { return "?page_id=317"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float3				scale;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;

			public void Execute(int vi)
			{
				jsverts[vi] = jvertices[vi] * scale;
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.scale		= scale;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p.x *= scale.x;
			p.y *= scale.y;
			p.z *= scale.z;
			return p;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaScale mod = root as MegaScale;

			if ( mod )
			{
				scale		= mod.scale;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}