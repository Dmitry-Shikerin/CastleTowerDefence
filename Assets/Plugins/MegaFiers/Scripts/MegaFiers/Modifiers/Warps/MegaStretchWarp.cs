using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Stretch")]
	public class MegaStretchWarp : MegaWarp
	{
		public float	amount		= 0.0f;
		public bool		doRegion	= false;
		public float	to			= 0.0f;
		public float	from		= 0.0f;
		public float	amplify		= 0.0f;
		public MegaAxis	axis		= MegaAxis.X;
		float			heightMax	= 0.0f;
		float			heightMin	= 0.0f;
		float			amplifier	= 0.0f;
		Matrix4x4		mat			= new Matrix4x4();
		Job				job;
		JobHandle		jobHandle;

		public override string WarpName() { return "Stretch"; }
		public override string GetIcon() { return "MegaStretch icon.png"; }
		public override string GetHelpURL() { return "?page_id=2560"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				amount;
			public bool					doRegion;
			public float				from;
			public float				to;
			public float				heightMax;
			public float				heightMin;
			public float				amplifier;
			public float				totaldecay;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			wtm;
			public Matrix4x4			winvtm;

			public void Execute(int vi)
			{
				float normHeight;
				float xyScale, zScale;

				if ( amount == 0.0f || (heightMax - heightMin == 0) )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				if ( (doRegion) && (to - from == 0.0f) )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				float3 p = wtm.MultiplyPoint3x4(jvertices[vi]);

				p = tm.MultiplyPoint3x4(p);

				float3 ip = p;
				float dist = math.length(p);
				float dcy = math.exp(-totaldecay * dist);

				if ( doRegion && p.y > to )
					normHeight = (to - heightMin) / (heightMax - heightMin);
				else if ( doRegion && p.y < from )
					normHeight = (from - heightMin) / (heightMax - heightMin);
				else
					normHeight = (p.y - heightMin) / (heightMax - heightMin);

				if ( amount < 0.0f )
				{
					xyScale = (amplifier * -amount + 1.0F);
					zScale = (-1.0f / (amount - 1.0F));
				}
				else
				{
					xyScale = 1.0f / (amplifier * amount + 1.0f);
					zScale = amount + 1.0f;
				}

				float a = 4.0f * (1.0f - xyScale);
				float b = -4.0f * (1.0f - xyScale);
				float c = 1.0f;
				float fraction = (((a * normHeight) + b) * normHeight) + c;
				p.x *= fraction;
				p.z *= fraction;

				if ( doRegion && p.y < from )
					p.y += (zScale - 1.0f) * from;
				else if ( doRegion && p.y <= to )
					p.y *= zScale;
				else if ( doRegion && p.y > to )
					p.y += (zScale - 1.0f) * to;
				else
					p.y *= zScale;

				p = math.lerp(ip, p, dcy);

				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.amount		= amount;
				job.doRegion	= doRegion;
				job.from		= from;
				job.to			= to;
				job.heightMax	= heightMax;
				job.heightMin	= heightMin;
				job.amplifier	= amplifier;
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

		void CalcBulge(MegaAxis axis, float stretch, float amplify)
		{
			amount = stretch;
			amplifier = (amplify >= 0.0f) ? amplify + 1.0f : 1.0f / (-amplify + 1.0f);

			if ( !doRegion )
			{
				switch ( axis )
				{
					case MegaAxis.X:
						heightMin = -Width * 0.5f;
						heightMax = Width * 0.5f;
						break;

					case MegaAxis.Z:
						heightMin = 0.0f;
						heightMax = Height;
						break;

					case MegaAxis.Y:
						heightMin = -Length * 0.5f;
						heightMax = Length * 0.5f;
						break;
				}
			}
			else
			{
				heightMin = from;
				heightMax = to;
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			float normHeight;
			float xyScale, zScale;

			if ( amount == 0.0f || (heightMax - heightMin == 0) )
				return p;

			if ( (doRegion) && (to - from == 0.0f) )
				return p;

			p = tm.MultiplyPoint3x4(p);

			Vector3 ip = p;
			float dist = p.magnitude;
			float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

			if ( doRegion && p.y > to )
				normHeight = (to - heightMin) / (heightMax - heightMin);
			else if ( doRegion && p.y < from )
				normHeight = (from - heightMin) / (heightMax - heightMin);
			else
				normHeight = (p.y - heightMin) / (heightMax - heightMin);

			if ( amount < 0.0f )
			{
				xyScale = (amplifier * -amount + 1.0F);
				zScale = (-1.0f / (amount - 1.0F));
			}
			else
			{
				xyScale = 1.0f / (amplifier * amount + 1.0f);
				zScale = amount + 1.0f;
			}

			float a = 4.0f * (1.0f - xyScale);
			float b = -4.0f * (1.0f - xyScale);
			float c = 1.0f;
			float fraction = (((a * normHeight) + b) * normHeight) + c;
			p.x *= fraction;
			p.z *= fraction;

			if ( doRegion && p.y < from )
				p.y += (zScale - 1.0f) * from;
			else if ( doRegion && p.y <= to )
				p.y *= zScale;
			else if ( doRegion && p.y > to )
				p.y += (zScale - 1.0f) * to;
			else
				p.y *= zScale;

			p = Vector3.Lerp(ip, p, dcy);

			p = invtm.MultiplyPoint3x4(p);

			return p;
		}

		public override bool Prepare(float decay)
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;

			mat = Matrix4x4.identity;

			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			SetAxis(mat);
			CalcBulge(axis, amount, amplify);

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