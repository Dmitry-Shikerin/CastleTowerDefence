using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Sinus Curve")]
	public class MegaSinusCurve : MegaModifier
	{
		[Adjust]
		public float		scale	= 1.0f;
		[Adjust]
		public float		wave	= 1.0f;
		[Adjust]
		public float		speed	= 1.0f;
		[Adjust]
		public float		phase	= 0.0f;
		[Adjust]
		public bool			animate	= false;
		Matrix4x4			mat		= new Matrix4x4();
		Job					job;
		JobHandle			jobHandle;

		public override string ModName() { return "Sinus Curve"; }
		public override string GetHelpURL() { return "Bubble.htm"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				phase;
			public float				wave;
			public float				scale;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				p.y += math.sin(phase + (p.x * wave) + p.y + p.z) * scale;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.phase		= phase;
				job.wave		= wave;
				job.scale		= scale;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			p.y += Mathf.Sin(phase + (p.x * wave) + p.y + p.z) * scale;

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( animate )
			{
				if ( Application.isPlaying )
					phase += Time.deltaTime * speed;
			}

			mat = Matrix4x4.identity;

			SetAxis(mat);
			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaSinusCurve mod = root as MegaSinusCurve;

			if ( mod )
			{
				scale		= mod.scale;
				wave		= mod.wave;
				speed		= mod.speed;
				phase		= mod.phase;
				animate		= false;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}