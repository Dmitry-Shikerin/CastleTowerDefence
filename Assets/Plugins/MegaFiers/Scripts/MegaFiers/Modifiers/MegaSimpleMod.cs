using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Simple")]
	public class MegaSimpleMod : MegaModifier
	{
		[Adjust]
		public Vector3	a3;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName()	{ return "Simple"; }
		public override string GetHelpURL() { return "?page_id=317"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float3				a3;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;

			public void Execute(int vi)
			{
				float3 p = jvertices[vi];

				p.x += (-1.0f + (UnityEngine.Random.value * 0.5f)) * a3.x;
				p.y += (-1.0f + (UnityEngine.Random.value * 0.5f)) * a3.y;
				p.z += (-1.0f + (UnityEngine.Random.value * 0.5f)) * a3.z;

				jsverts[vi] = p;
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.a3			= a3;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p.x += (-1.0f + (UnityEngine.Random.value * 0.5f)) * a3.x;
			p.y += (-1.0f + (UnityEngine.Random.value * 0.5f)) * a3.y;
			p.z += (-1.0f + (UnityEngine.Random.value * 0.5f)) * a3.z;
			return p;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaSimpleMod mod = root as MegaSimpleMod;

			if ( mod )
			{
				a3			= mod.a3;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}