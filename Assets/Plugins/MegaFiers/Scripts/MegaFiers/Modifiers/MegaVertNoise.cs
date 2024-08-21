using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Vertical Noise")]
	public class MegaVertNoise : MegaModifier
	{
		[Adjust]
		public float	Scale		= 1.0f;
		[Adjust]
		public float	Freq		= 0.25f;
		[Adjust]
		public bool		Animate		= false;
		[Adjust]
		public float	Phase		= 0.0f;
		[Adjust]
		public float	Strength	= 0.0f;
		float			time		= 0.0f;
		float			scale;
		Vector3			n			= Vector3.zero;
		[Adjust]
		public float	decay		= 0.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Vertical Noise"; }
		public override string GetHelpURL() { return "?page_id=1063"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				scale;
			public float				decay;
			public float				time;
			public float				Strength;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			float3						n;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float ipy = p.y;

				n.x = p.x * scale + 0.5f;
				n.y = p.z * scale + 0.5f;
				n.z = time;

				float dist = Mathf.Sqrt(p.x * p.x + p.z * p.z);
				float dcy = Mathf.Exp(-decay * Mathf.Abs(dist));

				float dy = 0.0f;
				dy = noise.snoise(n);

				p.y += dy * Strength;
				p.y = ipy + ((p.y - ipy) * dcy);

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.scale		= scale;
				job.decay		= decay;
				job.time		= time;
				job.Strength	= Strength;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override void ModStart(MegaModifyObject mc)
		{
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);
			float ipy = p.y;

			float dist = Mathf.Sqrt(p.x * p.x + p.z * p.z);
			float dcy = Mathf.Exp(-decay * Mathf.Abs(dist));

			float spx = p.x * scale + 0.5f;
			float spz = p.z * scale + 0.5f;

			float dy = 0.0f;

			n.x = p.x * scale + 0.5f;
			n.y = p.z * scale + 0.5f;
			n.z = time;

			dy = noise.snoise(n);

			p.y += dy * Strength;
			p.y = ipy + ((p.y - ipy) * dcy);

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( Animate )
			{
				if ( Application.isPlaying )
					Phase += Time.deltaTime * Freq;
			}
			time = Phase;

			if ( Scale == 0.0f )
				scale = 0.000001f;
			else
				scale = 1.0f / Scale;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaVertNoise mod = root as MegaVertNoise;

			if ( mod )
			{
				Scale		= mod.Scale;
				Freq		= mod.Freq;
				Animate		= false;
				Phase		= mod.Phase;
				Strength	= mod.Strength;
				decay		= mod.decay;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}