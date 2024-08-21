using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	public enum MegaMeltMat
	{
		Ice = 0,
		Glass,
		Jelly,
		Plastic,
		Custom,
	}

	[AddComponentMenu("Modifiers/Melt")]
	public class MegaMelt : MegaModifier
	{
		[Adjust]
		public float		Amount			= 0.0f;
		[Adjust]
		public float		Spread			= 19.0f;
		public MegaMeltMat	MaterialType	= MegaMeltMat.Ice;
		[Adjust]
		public float		Solidity		= 1.0f;
		[Adjust]
		public MegaAxis		axis			= MegaAxis.X;
		[Adjust]
		public bool			FlipAxis		= false;
		float				zba				= 0.0f;
		[Adjust]
		public float		flatness		= 0.0f;
		float				size			= 0.0f;
		float				bulger			= 0.0f;
		float				ybr,zbr,visvaluea;
		int					confiner,vistypea;
		float				cx,cy,cz;
		float				xsize,ysize,zsize;
		float				ooxsize,ooysize,oozsize;
		Job					job;
		JobHandle			jobHandle;

		public override string ModName() { return "Melt"; }
		public override string GetHelpURL() { return "?page_id=225"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				bulger;
			public MegaAxis				axis;
			public float				size;
			public float3				min;
			public float3				max;
			public float				cx;
			public float				cy;
			public float				cz;
			public bool					FlipAxis;
			public float				zbr;
			public float				visvaluea;
			public float				flatness;
			public float				ooxsize;
			public float				ooysize;
			public float				oozsize;
			public float				ybr;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			float hypot(float x, float y)
			{
				return math.sqrt(x * x + y * y);
			}

			public void Execute(int vi)
			{
				float x, y, z;
				float xw, yw, zw;
				float vdist, mfac, dx, dy;
				float defsinex = 0.0f, coldef = 0.0f, realmax = 0.0f;

				Vector3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				x = p.x; y = p.y; z = p.z;
#if true
				xw = x - cx; yw = y - cy; zw = z - cz;

				if ( xw == 0.0f && yw == 0.0f && zw == 0.0f ) xw = yw = zw = 1.0f;
				if ( x == 0.0f && y == 0.0f && z == 0.0f ) x = y = z = 1.0f;

				vdist = Mathf.Sqrt(xw * xw + yw * yw + zw * zw);

				mfac = size / vdist;

				switch ( axis )
				{
					case MegaAxis.X:
						dx = zw + Mathf.Sign(zw) * ((Mathf.Abs(zw * mfac)) * (bulger * ybr));
						dy = yw + Mathf.Sign(yw) * ((Mathf.Abs(yw * mfac)) * (bulger * ybr));
						z = (dx + cz);
						y = (dy + cy);
						break;
					case MegaAxis.Y:
						dx = xw + Mathf.Sign(xw) * ((Mathf.Abs(xw * mfac)) * (bulger * ybr));
						dy = yw + Mathf.Sign(yw) * ((Mathf.Abs(yw * mfac)) * (bulger * ybr));
						x = (dx + cx);
						y = (dy + cy);
						break;
					case MegaAxis.Z:
						dx = xw + Mathf.Sign(xw) * ((Mathf.Abs(xw * mfac)) * (bulger * ybr));
						dy = zw + Mathf.Sign(zw) * ((Mathf.Abs(zw * mfac)) * (bulger * ybr));
						x = (dx + cx);
						z = (dy + cz);
						break;
				}

				if ( axis == MegaAxis.Y ) if ( p.z < (min.z + zbr) ) goto skipmelt;
				if ( axis == MegaAxis.Z ) if ( p.y < (min.y + zbr) ) goto skipmelt;
				if ( axis == MegaAxis.X ) if ( p.x < (min.x + zbr) ) goto skipmelt;

				if ( axis == MegaAxis.Y ) realmax = hypot((max.x - cx), (max.y - cy));
				if ( axis == MegaAxis.Z ) realmax = hypot((max.x - cx), (max.z - cz));
				if ( axis == MegaAxis.X ) realmax = hypot((max.z - cz), (max.y - cy));

				switch ( axis )
				{
					case MegaAxis.X:
						defsinex = hypot((z - cz), (y - cy));
						coldef = realmax - hypot((z - cz), (y - cy));
						break;
					case MegaAxis.Y:
						defsinex = hypot((x - cx), (y - cy));
						coldef = realmax - hypot((x - cx), (y - cy));
						break;
					case MegaAxis.Z:
						defsinex = hypot((x - cx), (z - cz));
						coldef = realmax - hypot((x - cx), (z - cz));
						break;
					default:
						break;
				}

				if ( coldef < 0.0f )
					coldef = 0.0f;

				defsinex += (coldef / visvaluea);

				switch ( axis )
				{
					case MegaAxis.X:
						if ( !FlipAxis )
						{
							float nminx = min.x + (((x - min.x) * ooxsize) * flatness);
							x -= (defsinex * bulger);
							if ( x <= nminx ) x = nminx;
							if ( x <= (nminx + zbr) ) x = (nminx + zbr);
						}
						else
						{
							float nmaxx = max.x - (((max.x - x) * ooxsize) * flatness);
							x += (defsinex * bulger);
							if ( x >= nmaxx ) x = nmaxx;
							if ( x >= (nmaxx + zbr) ) x = (nmaxx + zbr);
						}
						break;
					case MegaAxis.Y:
						if ( FlipAxis )
						{
							float nminz = min.z + (((z - min.z) * oozsize) * flatness);

							z -= (defsinex * bulger);
							if ( z <= nminz ) z = nminz;
							if ( z <= (nminz + zbr) ) z = (nminz + zbr);
						}
						else
						{
							float nmaxz = max.z - (((max.z - z) * oozsize) * flatness);

							z += (defsinex * bulger);
							if ( z >= nmaxz ) z = nmaxz;
							if ( z >= (nmaxz + zbr) ) z = (nmaxz + zbr);
						}
						break;
					case MegaAxis.Z:
						if ( !FlipAxis )
						{
							float nminy = min.y + (((y - min.y) * ooysize) * flatness);
							y -= (defsinex * bulger);
							if ( y <= nminy ) y = nminy;
							if ( y <= (nminy + zbr) ) y = (nminy + zbr);
						}
						else
						{
							float nmaxy = max.y - (((max.y - y) * ooysize) * flatness);
							y += (defsinex * bulger);
							if ( y >= nmaxy ) y = nmaxy;
							if ( y >= (nmaxy + zbr) ) y = (nmaxy + zbr);
						}
						break;
					default:
						break;
				}
#endif
			// [jump point] don't melt this point...
			skipmelt:
				p.x = x; p.y = y; p.z = z;
				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = p;	//invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.bulger		= bulger;
				job.axis		= axis;
				job.size		= size;
				job.min			= bbox.min;
				job.max			= bbox.max;
				job.cx			= cx;
				job.cy			= cy;
				job.cz			= cz;
				job.FlipAxis	= FlipAxis;
				job.zbr			= zbr;
				job.visvaluea	= visvaluea;
				job.flatness	= flatness;
				job.ooxsize		= ooxsize;
				job.ooysize		= ooysize;
				job.oozsize		= oozsize;
				job.ybr			= ybr;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		float hypot(float x, float y)
		{
			return Mathf.Sqrt(x * x + y * y);
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			float x, y, z;
			float xw,yw,zw;
			float vdist,mfac,dx,dy;
			float defsinex = 0.0f, coldef = 0.0f, realmax = 0.0f;

			p = tm.MultiplyPoint3x4(p);
			x = p.x; y = p.y; z = p.z;
#if true
			xw = x - cx; yw = y - cy; zw = z - cz;

			if ( xw == 0.0f && yw == 0.0f && zw == 0.0f ) xw = yw = zw = 1.0f;
			if ( x == 0.0f && y == 0.0f && z == 0.0f ) x = y = z = 1.0f;

			vdist = Mathf.Sqrt(xw * xw + yw * yw + zw * zw);

			mfac = size / vdist;

			switch ( axis )
			{
				case MegaAxis.X:
					dx = zw + Mathf.Sign(zw) * ((Mathf.Abs(zw * mfac)) * (bulger * ybr));
					dy = yw + Mathf.Sign(yw) * ((Mathf.Abs(yw * mfac)) * (bulger * ybr));
					z = (dx + cz);
					y = (dy + cy);
					break;
				case MegaAxis.Y:
					dx = xw + Mathf.Sign(xw) * ((Mathf.Abs(xw * mfac)) * (bulger * ybr));
					dy = yw + Mathf.Sign(yw) * ((Mathf.Abs(yw * mfac)) * (bulger * ybr));
					x = (dx + cx);
					y = (dy + cy);
					break;
				case MegaAxis.Z:
					dx = xw + Mathf.Sign(xw) * ((Mathf.Abs(xw * mfac)) * (bulger * ybr));
					dy = zw + Mathf.Sign(zw) * ((Mathf.Abs(zw * mfac)) * (bulger * ybr));
					x = (dx + cx);
					z = (dy + cz);
					break;
			}

			if ( axis == MegaAxis.Y ) if ( p.z < (bbox.min.z + zbr) ) goto skipmelt;
			if ( axis == MegaAxis.Z ) if ( p.y < (bbox.min.y + zbr) ) goto skipmelt;
			if ( axis == MegaAxis.X ) if ( p.x < (bbox.min.x + zbr) ) goto skipmelt;

			if ( axis == MegaAxis.Y ) realmax = hypot((bbox.max.x - cx), (bbox.max.y - cy));
			if ( axis == MegaAxis.Z ) realmax = hypot((bbox.max.x - cx), (bbox.max.z - cz));
			if ( axis == MegaAxis.X ) realmax = hypot((bbox.max.z - cz), (bbox.max.y - cy));

			switch ( axis )
			{
				case MegaAxis.X:
					defsinex = hypot((z - cz), (y - cy));
					coldef = realmax - hypot((z - cz), (y - cy));
					break;
				case MegaAxis.Y:
					defsinex = hypot((x - cx), (y - cy));
					coldef = realmax - hypot((x - cx), (y - cy));
					break;
				case MegaAxis.Z:
					defsinex = hypot((x - cx), (z - cz));
					coldef = realmax - hypot((x - cx), (z - cz));
					break;
				default:
					break;
			}

			if ( coldef < 0.0f )
				coldef = 0.0f;

			defsinex += (coldef / visvaluea);

			switch ( axis )
			{
				case MegaAxis.X:
					if ( !FlipAxis )
					{
						float nminx = bbox.min.x + (((x - bbox.min.x) * ooxsize) * flatness);
						x -= (defsinex * bulger);
						if ( x <= nminx ) x = nminx;
						if ( x <= (nminx + zbr) ) x = (nminx + zbr);
					}
					else
					{
						float nmaxx = bbox.max.x - (((bbox.max.x - x) * ooxsize) * flatness);
						x += (defsinex * bulger);
						if ( x >= nmaxx ) x = nmaxx;
						if ( x >= (nmaxx + zbr) ) x = (nmaxx + zbr);
					}
					break;
				case MegaAxis.Y:
					if ( FlipAxis )
					{
						float nminz = bbox.min.z + (((z - bbox.min.z) * oozsize) * flatness);

						z -= (defsinex * bulger);
						if ( z <= nminz ) z = nminz;
						if ( z <= (nminz + zbr) ) z = (nminz + zbr);
					}
					else
					{
						float nmaxz = bbox.max.z - (((bbox.max.z - z) * oozsize) * flatness);

						z += (defsinex * bulger);
						if ( z >= nmaxz ) z = nmaxz;
						if ( z >= (nmaxz + zbr) ) z = (nmaxz + zbr);
					}
					break;
				case MegaAxis.Z:
					if ( !FlipAxis )
					{
						float nminy = bbox.min.y + (((y - bbox.min.y) * ooysize) * flatness);
						y -= (defsinex * bulger);
						if ( y <= nminy ) y = nminy;
						if ( y <= (nminy + zbr) ) y = (nminy + zbr);
					}
					else
					{
						float nmaxy = bbox.max.y - (((bbox.max.y - y) * ooysize) * flatness);
						y += (defsinex * bulger);
						if ( y >= nmaxy ) y = nmaxy;
						if ( y >= (nmaxy + zbr) ) y = (nmaxy + zbr);
					}
					break;
				default:
					break;
			}
#endif
		// [jump point] don't melt this point...
		skipmelt:
			p.x = x; p.y = y; p.z = z;
			p = invtm.MultiplyPoint3x4(p);
			return p;
		}

		public override void ModStart(MegaModifyObject mc)
		{
			cx = bbox.center.x;
			cy = bbox.center.y;
			cz = bbox.center.z;

			// Compute the size and center
			xsize = (bbox.max.x - bbox.min.x);
			ysize = (bbox.max.y - bbox.min.y);
			zsize = (bbox.max.z - bbox.min.z);

			size = (xsize > ysize) ? xsize : ysize;
			size = (zsize > size) ? zsize : size;
			size /= 2.0f;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			cx = bbox.center.x;
			cy = bbox.center.y;
			cz = bbox.center.z;

			// Compute the size and center
			xsize = (bbox.max.x - bbox.min.x);
			ysize = (bbox.max.y - bbox.min.y);
			zsize = (bbox.max.z - bbox.min.z);

			ooxsize = 1.0f / xsize;
			ooysize = 1.0f / ysize;
			oozsize = 1.0f / zsize;

			size = (xsize > ysize) ? xsize : ysize;
			size = (zsize > size) ? zsize : size;
			size /= 2.0f;

			switch ( MaterialType )
			{
				case MegaMeltMat.Ice:		visvaluea = 2.0f;		break;
				case MegaMeltMat.Glass:		visvaluea = 12.0f;		break;
				case MegaMeltMat.Jelly:		visvaluea = 0.4f;		break;
				case MegaMeltMat.Plastic:	visvaluea = 0.7f;		break;
				case MegaMeltMat.Custom:	visvaluea = Solidity;	break;
			}

			if ( Amount < 0.0f )
				Amount = 0.0f;

			ybr = Spread / 100.0f;
			zbr = zba / 10.0f;
			bulger = Amount / 100.0f;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaMelt mod = root as MegaMelt;

			if ( mod )
			{
				Amount			= mod.Amount;
				Spread			= mod.Spread;
				MaterialType	= mod.MaterialType;
				Solidity		= mod.Solidity;
				axis			= mod.axis;
				FlipAxis		= mod.FlipAxis;
				flatness		= mod.flatness;
				gizmoPos		= mod.gizmoPos;
				gizmoRot		= mod.gizmoRot;
				gizmoScale		= mod.gizmoScale;
				bbox			= mod.bbox;
			}
		}
	}
}