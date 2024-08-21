using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Hump")]
	public class MegaHump : MegaModifier
	{
		[Adjust]
		public float	amount	= 0.0f;
		[Adjust]
		public float	cycles	= 1.0f;
		[Adjust]
		public float	phase	= 0.0f;
		[Adjust]
		public bool		animate	= false;
		[Adjust]
		public float	speed	= 1.0f;
		[Adjust]
		public MegaAxis	axis	= MegaAxis.Z;
		float			amt;
		Vector3			size = Vector3.zero;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Hump"; }
		public override string GetHelpURL() { return "?page_id=207"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				amt;
			public float3				size;
			public float				phase;
			public float				cycles;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public MegaAxis				axis;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				Vector3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				switch ( axis )
				{
					case MegaAxis.X: p.x += amt * math.sin(math.sqrt(p.x * p.x / size.x) + math.sqrt(p.y * p.y / size.y) * math.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
					case MegaAxis.Y: p.y += amt * math.sin(math.sqrt(p.y * p.y / size.y) + math.sqrt(p.x * p.x / size.x) * math.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
					case MegaAxis.Z: p.z += amt * math.sin(math.sqrt(p.x * p.x / size.x) + math.sqrt(p.y * p.y / size.y) * math.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
				}

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.amt			= amt;
				job.axis		= axis;
				job.size		= size;
				job.cycles		= cycles;
				job.phase		= phase;
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

			switch ( axis )
			{
				case MegaAxis.X: p.x += amt * Mathf.Sin(Mathf.Sqrt(p.x * p.x / size.x) + Mathf.Sqrt(p.y * p.y / size.y) * Mathf.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
				case MegaAxis.Y: p.y += amt * Mathf.Sin(Mathf.Sqrt(p.y * p.y / size.y) + Mathf.Sqrt(p.x * p.x / size.x) * Mathf.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
				case MegaAxis.Z: p.z += amt * Mathf.Sin(Mathf.Sqrt(p.x * p.x / size.x) + Mathf.Sqrt(p.y * p.y / size.y) * Mathf.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
			}
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

			size = bbox.Size();
			amt = amount / 100.0f;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaHump mod = root as MegaHump;

			if ( mod )
			{
				amount		= mod.amount;
				cycles		= mod.cycles;
				phase		= mod.phase;
				animate		= false;
				speed		= mod.speed;
				axis		= mod.axis;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}