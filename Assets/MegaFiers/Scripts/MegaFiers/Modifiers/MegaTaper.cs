using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	public enum MegaEffectAxis
	{
		X = 0,
		Y = 1,
		XY = 2,
	};

	[AddComponentMenu("Modifiers/Taper")]
	public class MegaTaper : MegaModifier
	{
		[Adjust]
		public float			amount		= 0.0f;
		[Adjust]
		public bool				doRegion	= false;
		[Adjust]
		public float			to			= 0.0f;
		[Adjust]
		public float			from		= 0.0f;
		[Adjust]
		public float			dir			= 0.0f;
		[Adjust]
		public MegaAxis			axis		= MegaAxis.Z;
		public MegaEffectAxis	EAxis		= MegaEffectAxis.X;
		Matrix4x4				mat			= new Matrix4x4();
		[Adjust]
		public float			crv			= 0.0f;
		[Adjust]
		public bool				sym			= false;
		bool					doX			= false;
		bool					doY			= false;
		float					k1;
		float					k2;
		float					l;
		float					ovl;
		Job						job;
		JobHandle				jobHandle;

		void SetK(float K1, float K2)
		{
			k1 = K1;
			k2 = K2;
		}

		public override string ModName() { return "Taper"; }
		public override string GetHelpURL() { return "?page_id=338"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public bool					doRegion;
			public float				l;
			public float				from;
			public float				to;
			public float				ovl;
			public bool					sym;
			public float				k1;
			public float				k2;
			public bool					doX;
			public bool					doY;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float z;

				if ( l == 0.0f )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				if ( doRegion )
				{
					if ( p.y < from )
						z = from * ovl;
					else
					{
						if ( p.y > to )
							z = to * ovl;
						else
							z = p.y * ovl;
					}
				}
				else
					z = p.y * ovl;

				if ( sym && z < 0.0f )
					z = -z;

				float f = 1.0f + z * k1 + k2 * z * (1.0f - z);

				if ( doX )
					p.x *= f;

				if ( doY )
					p.z *= f;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.l			= l;
				job.doRegion	= doRegion;
				job.from		= from;
				job.to			= to;
				job.ovl			= ovl;
				job.sym			= sym;
				job.k1			= k1;
				job.k2			= k2;
				job.doX			= doX;
				job.doY			= doY;
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
			float z;

			if ( l == 0.0f )
				return p;

			p = tm.MultiplyPoint3x4(p);

			if ( doRegion )
			{
				if ( p.y < from )
					z = from * ovl;
				else
				{
					if ( p.y > to )
						z = to * ovl;
					else
						z = p.y * ovl;
				}
			}
			else
				z = p.y * ovl;

			if ( sym && z < 0.0f )
				z = -z;

			float f =  1.0f + z * k1 + k2 * z * (1.0f - z);	

			if ( doX )
				p.x *= f;

  			if ( doY )
				p.z *= f;

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			switch ( EAxis )
			{
				case MegaEffectAxis.X:	doX = true;		doY = false;	break;
				case MegaEffectAxis.Y:	doX = false;	doY = true;		break;
				case MegaEffectAxis.XY: doX = true;		doY = true;		break;
			}

			mat = Matrix4x4.identity;
			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f);	l = bbox.max[0] - bbox.min[0];	break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); l = bbox.max[2] - bbox.min[2];	break;
				case MegaAxis.Z: l = bbox.max[1] - bbox.min[1]; break;
			}

			if ( l != 0.0f )
				ovl = 1.0f / l;

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir);

			SetAxis(mat);
			SetK(amount, crv);

			return true;
		}

		public override void ExtraGizmo(MegaModContext mc)
		{
			if ( doRegion )
				DrawFromTo(axis, from , to, mc);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaTaper mod = root as MegaTaper;

			if ( mod )
			{
				amount		= mod.amount;
				doRegion	= mod.doRegion;
				to			= mod.to;
				from		= mod.from;
				dir			= mod.dir;
				axis		= mod.axis;
				EAxis		= mod.EAxis;
				crv			= mod.crv;
				sym			= mod.sym;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}