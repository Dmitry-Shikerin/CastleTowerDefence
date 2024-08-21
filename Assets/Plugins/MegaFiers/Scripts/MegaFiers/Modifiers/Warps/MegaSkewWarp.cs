using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Skew")]
	public class MegaSkewWarp : MegaWarp
	{
		public float	amount		= 0.0f;
		public bool		doRegion	= false;
		public float	to			= 0.0f;
		public float	from		= 0.0f;
		public float	dir			= 0.0f;
		public MegaAxis	axis		= MegaAxis.X;
		Matrix4x4		mat			= new Matrix4x4();
		float			amountOverLength = 0.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string WarpName() { return "Skew"; }
		public override string GetIcon() { return "MegaSkew icon.png"; }
		public override string GetHelpURL() { return "?page_id=2571"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public bool					doRegion;
			public float				to;
			public float				from;
			public float				amountOverLength;
			public float				totaldecay;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			wtm;
			public Matrix4x4			winvtm;

			public void Execute(int vi)
			{
				float3 p = wtm.MultiplyPoint3x4(jvertices[vi]);

				p = tm.MultiplyPoint3x4(p);

				float3 ip = p;
				float dist = math.length(p);
				float dcy = math.exp(-totaldecay * dist);

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

				p = math.lerp(ip, p, dcy);

				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.doRegion			= doRegion;
				job.to					= to;
				job.from				= from;
				job.amountOverLength	= amountOverLength;
				job.totaldecay			= totaldecay;
				job.tm					= tm;
				job.invtm				= invtm;
				job.wtm					= mod.tm;
				job.winvtm				= mod.invtm;
				job.jvertices			= mod.jverts;
				job.jsverts				= mod.jsverts;

				jobHandle = job.Schedule(mod.jverts.Length, 64);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			Vector3 ip = p;
			float dist = p.magnitude;
			float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

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

			p = Vector3.Lerp(ip, p, dcy);

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool Prepare(float decay)
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;

			if ( from > 0.0f )
				from = 0.0f;

			if ( to < 0.0f )
				to = 0.0f;

			mat = Matrix4x4.identity;

			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
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
					case MegaAxis.X: len = Width; break;
					case MegaAxis.Z: len = Height; break;
					case MegaAxis.Y: len = Length; break;
				}
			}
			else
				len = to - from;

			if ( len == 0.0f )
				len = 0.000001f;

			amountOverLength = amount / len;

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