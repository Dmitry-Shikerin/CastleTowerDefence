using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Rolled")]
	public class MegaRolled : MegaModifier
	{
		[Adjust]
		public float		radius			= 1.0f;
		public Transform	roller;
		[Adjust]
		public float		splurge			= 1.0f;
		[Adjust]
		public MegaAxis		fwdaxis			= MegaAxis.Z;
		Matrix4x4			mat				= new Matrix4x4();
		Vector3[]			offsets;
		Plane				plane;
		float				height			= 0.0f;
		Job					job;
		JobHandle			jobHandle;
		Vector3				rpos;
		public bool			clearoffsets	= false;
		float				delta			= 0.0f;

		public override string ModName() { return "Rolled"; }
		public override string GetHelpURL() { return "?page_id=1292"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				delta;
			public float				splurge;
			public float3				rpos;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				if ( p.z > rpos.z )
				{
					p.y *= delta;
					p.x += (1.0f - delta) * splurge * p.x;
					p.z += (1.0f - delta) * splurge * (p.z - rpos.z);
				}

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.delta		= delta;
				job.splurge		= splurge;
				job.rpos		= rpos;
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
			if ( i >= 0 )
			{
				p = tm.MultiplyPoint3x4(p);

				if ( p.z > rpos.z )
				{
					p.y *= delta;

					p.x += (1.0f - delta) * splurge * p.x;
					p.z += (1.0f - delta) * splurge * (p.z - rpos.z);
				}

				p = invtm.MultiplyPoint3x4(p);
			}

			return p;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( !roller )
				return false;

			rpos = transform.worldToLocalMatrix.MultiplyPoint3x4(roller.position);

			height = rpos.y - radius;

			if ( offsets == null || offsets.Length != mc.mod.jverts.Length )
				offsets = new Vector3[mc.mod.jverts.Length];

			mat = Matrix4x4.identity;

			SetAxis(mat);
			tm = Matrix4x4.identity;

			if ( clearoffsets )
			{
				clearoffsets = false;

				for ( int i = 0; i < offsets.Length; i++ )
				{
					offsets[i] = Vector3.zero;
				}
			}

			if ( height < mc.bbox.Size().y )
				delta = height / mc.bbox.Size().y;
			else
				delta = 1.0f;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaRolled mod = root as MegaRolled;

			if ( mod )
			{
				radius		= mod.radius;
				roller		= mod.roller;
				splurge		= mod.splurge;
				fwdaxis		= mod.fwdaxis;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}