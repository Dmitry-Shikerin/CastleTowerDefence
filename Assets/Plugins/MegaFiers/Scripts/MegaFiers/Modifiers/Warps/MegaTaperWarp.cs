using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Taper")]
	public class MegaTaperWarp : MegaWarp
	{
		public override string WarpName() { return "Taper"; }
		public override string GetIcon() { return "MegaTaper icon.png"; }
		public override string GetHelpURL() { return "?page_id=2566"; }

		public float			amount		= 0.0f;
		public bool				doRegion	= false;
		public float			to			= 0.0f;
		public float			from		= 0.0f;
		public float			dir			= 0.0f;
		public MegaAxis			axis		= MegaAxis.X;
		public MegaEffectAxis	EAxis		= MegaEffectAxis.X;
		Matrix4x4				mat			= new Matrix4x4();
		public float			crv			= 0.0f;
		public bool				sym			= false;
		bool					doX			= false;
		bool					doY			= false;
		float					k1;
		float					k2;
		float					l;
		Job						job;
		JobHandle				jobHandle;

		void SetK(float K1, float K2)
		{
			k1 = K1;
			k2 = K2;
		}

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public bool					doRegion;
			public float				l;
			public float				from;
			public float				to;
			public bool					sym;
			public float				k1;
			public float				k2;
			public bool					doX;
			public bool					doY;
			public float				totaldecay;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			wtm;
			public Matrix4x4			winvtm;

			public void Execute(int vi)
			{
				if ( l == 0.0f )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				float3 p = wtm.MultiplyPoint3x4(jvertices[vi]);

				float z;

				p = tm.MultiplyPoint3x4(p);

				float3 ip = p;
				float dist = math.length(p);
				float dcy = math.exp(-totaldecay * dist);

				if ( doRegion )
				{
					if ( p.y < from )
						z = from / l;
					else
					{
						if ( p.y > to )
							z = to / l;
						else
							z = p.y / l;
					}
				}
				else
					z = p.y / l;

				if ( sym && z < 0.0f )
					z = -z;

				float f = 1.0f + z * k1 + k2 * z * (1.0f - z);

				if ( doX )
					p.x *= f;

				if ( doY )
					p.z *= f;

				p = math.lerp(ip, p, dcy);

				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.l			= l;
				job.doRegion	= doRegion;
				job.from		= from;
				job.to			= to;
				job.sym			= sym;
				job.k1			= k1;
				job.k2			= k2;
				job.doX			= doX;
				job.doY			= doY;
				job.totaldecay	= totaldecay;
				job.tm			= tm;
				job.invtm		= invtm;
				job.wtm			= mod.tm;
				job.winvtm		= mod.invtm;
				job.jvertices	= mod.jverts;
				job.jsverts		= mod.jsverts;

				jobHandle = job.Schedule(mod.jverts.Length, 64);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			float z;

			if ( l == 0.0f )
				return p;

			p = tm.MultiplyPoint3x4(p);

			Vector3 ip = p;
			float dist = p.magnitude;
			float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

			if ( doRegion )
			{
				if ( p.y < from )
					z = from / l;
				else
				{
					if ( p.y > to )
						z = to / l;
					else
						z = p.y / l;
				}
			}
			else
				z = p.y / l;

			if ( sym && z < 0.0f )
				z = -z;

			float f =  1.0f + z * k1 + k2 * z * (1.0f - z);

			if ( doX )
				p.x *= f;

			if ( doY )
				p.z *= f;

			p = Vector3.Lerp(ip, p, dcy);

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool Prepare(float decay)
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;

			switch ( EAxis )
			{
				case MegaEffectAxis.X: doX = true; doY = false; break;
				case MegaEffectAxis.Y: doX = false; doY = true; break;
				case MegaEffectAxis.XY: doX = true; doY = true; break;
			}

			mat = Matrix4x4.identity;
			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); l = Width; break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); l = Length; break;
				case MegaAxis.Z: l = Height; break;
			}

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir);

			SetAxis(mat);
			SetK(amount, crv);

			totaldecay = Decay + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;
			return true;
		}

		public override void ExtraGizmo()
		{
			if ( doRegion )
				DrawFromTo(axis, from, to);
		}
	}
}