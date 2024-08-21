using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Bubble")]
	public class MegaBubble : MegaModifier
	{
		[Adjust]
		public float		radius	= 0.0f;
		[Adjust]
		public float		falloff	= 20.0f;
		Matrix4x4			mat		= new Matrix4x4();
		BubbleJob			bubbleJob;
		JobHandle			jobHandle;

		public override string ModName()	{ return "Bubble"; }
		public override string GetHelpURL() { return "?page_id=111"; }

		[BurstCompile]
		struct BubbleJob : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public float				radius;
			public float				falloff;
			public Vector3				center;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int i)
			{
				Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

				float val = (p - center).magnitude / falloff;
				float3 n = math.normalize(p - center);
				val = val * val + 1.0f;
				p.x += radius * n.x / val;
				p.y += radius * n.y / val;
				p.z += radius * n.z / val;

				jsverts[i] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				bubbleJob.radius	= radius;
				bubbleJob.center	= bbox.center;
				bubbleJob.falloff	= falloff;
				bubbleJob.tm		= tm;
				bubbleJob.invtm		= invtm;
				bubbleJob.jvertices	= jverts;
				bubbleJob.jsverts	= jsverts;

				jobHandle = bubbleJob.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			float val = ((Vector3.Magnitude(p - bbox.center)) / falloff);
			p += radius * (Vector3.Normalize(p - bbox.center)) / (val * val + 1.0f);

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			mat = Matrix4x4.identity;

			SetAxis(mat);
			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaBubble mod = root as MegaBubble;

			if ( mod )
			{
				radius		= mod.radius;
				falloff		= mod.falloff;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}