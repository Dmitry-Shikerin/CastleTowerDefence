using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Skew")]
	public class MegaSkew : MegaModifier
	{
		[Adjust]
		public float	amount			= 0.0f;
		[Adjust]
		public bool		doRegion		= false;
		[Adjust]
		public float	to				= 0.0f;
		[Adjust]
		public float	from			= 0.0f;
		[Adjust]
		public float	dir				= 0.0f;
		[Adjust]
		public MegaAxis	axis			= MegaAxis.X;
		Matrix4x4		mat				= new Matrix4x4();
		float			amountOverLength = 0.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Skew"; }
		public override string GetHelpURL() { return "?page_id=319"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public bool					doRegion;
			public float				to;
			public float				from;
			public float				amountOverLength;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float z = p.y;

				if ( doRegion )
				{
					if ( p.y < from )
						z = from;
					else
						if ( p.y > to )
						z = to;
				}

				p.x -= z * amountOverLength;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.doRegion			= doRegion;
				job.to					= to;
				job.from				= from;
				job.amountOverLength	= amountOverLength;
				job.tm					= tm;
				job.invtm				= invtm;
				job.jvertices			= jverts;
				job.jsverts				= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);
			float z = p.y;

			if ( doRegion )
			{
				if ( p.y < from )
					z = from;
				else
					if ( p.y > to )
						z = to;
			}

			p.x -= z * amountOverLength;
			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( from > 0.0f )
				from = 0.0f;

			if ( to < 0.0f )
				to = 0.0f;

			mat = Matrix4x4.identity;

			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f);	break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir);
			SetAxis(mat);

			float len = 0.0f;
			if ( !doRegion )
			{
				switch ( axis )
				{
					case MegaAxis.X: len = bbox.max.x - bbox.min.x; break;
					case MegaAxis.Z: len = bbox.max.y - bbox.min.y; break;
					case MegaAxis.Y: len = bbox.max.z - bbox.min.z; break;
				}
			}
			else
				len = to - from;

			if ( len == 0.0f )
				len = 0.000001f;

			amountOverLength = amount / len;
			return true;
		}

		public override void ExtraGizmo(MegaModContext mc)
		{
			if ( doRegion )
				DrawFromTo(axis, from, to, mc);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaSkew mod = root as MegaSkew;

			if ( mod )
			{
				amount		= mod.amount;
				doRegion	= mod.doRegion;
				to			= mod.to;
				from		= mod.from;
				dir			= mod.dir;
				axis		= mod.axis;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}