using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Noise")]
	public class MegaNoise : MegaModifier
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
		public Vector3	Strength	= new Vector3(0.0f, 0.0f, 0.0f);
		float			time		= 0.0f;
		float			scale;
		Vector3			sp			= Vector3.zero;
		Vector3			n			= Vector3.zero;
		Vector3			d			= Vector3.zero;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Noise"; }
		public override string GetHelpURL() { return "?page_id=262"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float3				Strength;
			public float				time;
			public float				scale;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			float3						sp;
			float3						d;
			float3						n;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				sp = p * scale + 0.5f;
				n.x = sp.y;
				n.y = sp.z;
				n.z = time;
				d.x = noise.snoise(n);

				n.x = sp.x;
				n.y = sp.z;
				n.z = time;
				d.y = noise.snoise(n);

				n.x = sp.x;
				n.y = sp.y;
				n.z = time;
				d.z = noise.snoise(n);

				p += d * Strength;	//math.mul(d, Strength);

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.Strength	= Strength;
				job.time		= time;
				job.scale		= scale;
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

			sp.x = p.x * scale + 0.5f;
			sp.y = p.y * scale + 0.5f;
			sp.z = p.z * scale + 0.5f;

			if ( Strength.x != 0.0f )
			{
				n.x = sp.y;
				n.y = sp.z;
				n.z = time;
				p.x += noise.snoise(n) * Strength.x;
			}

			if ( Strength.y != 0.0f )
			{
				n.x = sp.x;
				n.y = sp.z;
				n.z = time;
				p.y += noise.snoise(n) * Strength.y;
			}

			if ( Strength.z != 0.0f )
			{
				n.x = sp.x;
				n.y = sp.y;
				n.z = time;
				p.z += noise.snoise(n) * Strength.z;
			}

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
			MegaNoise mod = root as MegaNoise;

			if ( mod )
			{
				Scale		= mod.Scale;
				Freq		= mod.Freq;
				Animate		= false;
				Phase		= mod.Phase;
				Strength	= mod.Strength;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}