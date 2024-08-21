using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Crumple")]
	public class MegaCrumple : MegaModifier
	{
		[Adjust]
		public float		scale	= 1.0f;
		[Adjust]
		public float		speed	= 1.0f;
		[Adjust]
		public float		phase	= 0.0f;
		[Adjust]
		public bool			animate	= false;
		Matrix4x4			mat		= new Matrix4x4();
		CrumpleJob			job;
		JobHandle			jobHandle;
		float				timex	= 0.0f;
		float				timey	= 0.0f;
		float				timez	= 0.0f;

		public override string ModName() { return "Crumple"; }
		public override string GetHelpURL() { return "?page_id=653"; }

		[BurstCompile]
		struct CrumpleJob : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public float				scale;
			public float				speed;
			public float				phase;
			public float3				time;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			float3 n;

			public void Execute(int i)
			{
				Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

				n.x = time.x + p.x;
				n.y = time.x + p.y;
				n.z = time.x + p.z;

				p.x += noise.snoise(n) * scale;

				n.x = time.y + p.x;
				n.y = time.y + p.y;
				n.z = time.y + p.z;

				p.y += noise.snoise(n) * scale;

				n.x = time.z + p.x;
				n.y = time.z + p.y;
				n.z = time.z + p.z;

				p.z += noise.snoise(n) * scale;

				jsverts[i] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.scale		= scale;
				job.speed		= speed;
				job.phase		= phase;
				job.time.x		= timex;
				job.time.y		= timey;
				job.time.z		= timez;
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

			Vector3 n = Vector3.zero;
			n.x = timex + p.x;
			n.y = timex + p.y;
			n.z = timex + p.z;

			p.x += noise.snoise(n) * scale;

			n.x = timey + p.x;
			n.y = timey + p.y;
			n.z = timey + p.z;

			p.y += noise.snoise(n) * scale;

			n.x = timez + p.x;
			n.y = timez + p.y;
			n.z = timez + p.z;

			p.z += noise.snoise(n) * scale;

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

			timex = 0.1365143f + phase;
			timey = 1.21688f + phase;
			timez = 2.5564f + phase;

			mat = Matrix4x4.identity;

			SetAxis(mat);
			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaCrumple mod = root as MegaCrumple;

			if ( mod )
			{
				scale		= mod.scale;
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