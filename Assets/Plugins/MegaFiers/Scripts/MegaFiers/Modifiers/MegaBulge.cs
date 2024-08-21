using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Bulge")]
	public class MegaBulge : MegaModifier
	{
		[Adjust]
		public Vector3	Amount		= Vector3.zero;
		[Adjust]
		public Vector3	FallOff		= Vector3.zero;
		[Adjust]
		public bool		LinkFallOff	= true;
		[Adjust]
		Vector3 per			= Vector3.zero;
		float			xsize;
		float			ysize;
		float			zsize;
		float			size;
		float			cx,cy,cz;
		Vector3			dcy			= Vector3.zero;
		BulgeJob		job;
		JobHandle		jobHandle;

		public override string ModName()	{ return "Bulge"; }
		public override string GetHelpURL() { return "?page_id=163"; }

		[BurstCompile]
		struct BulgeJob : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Vector3				falloff;
			public bool					linkfalloff;
			public float				cx;
			public float				cy;
			public float				cz;
			public float				size;
			public float3				per;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			float3						dcy;

			public void Execute(int i)
			{
				Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

				float xw, yw, zw;

				xw = p.x - cx; yw = p.y - cy; zw = p.z - cz;
				if ( xw == 0.0f && yw == 0.0f && zw == 0.0f )
					xw = yw = zw = 1.0f;
				float vdist = math.sqrt(xw * xw + yw * yw + zw * zw);
				float mfac = size / vdist;

				dcy.x = math.exp(-falloff.x * Mathf.Abs(xw));

				if ( !linkfalloff )
				{
					dcy.y = math.exp(-falloff.y * Mathf.Abs(yw));
					dcy.z = math.exp(-falloff.z * Mathf.Abs(zw));
				}
				else
					dcy.y = dcy.z = dcy.x;

				p.x = cx + xw + (math.sign(xw) * ((math.abs(xw * mfac) - math.abs(xw)) * per.x) * dcy.x);
				p.y = cy + yw + (math.sign(yw) * ((math.abs(yw * mfac) - math.abs(yw)) * per.y) * dcy.y);
				p.z = cz + zw + (math.sign(zw) * ((math.abs(zw * mfac) - math.abs(zw)) * per.z) * dcy.z);

				jsverts[i] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.falloff		= FallOff;
				job.linkfalloff	= LinkFallOff;
				job.cx			= cx;
				job.cy			= cy;
				job.cz			= cz;
				job.size		= size;
				job.per			= per;
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

			float xw,yw,zw;

			xw = p.x - cx; yw = p.y - cy; zw = p.z - cz;
			if ( xw == 0.0f && yw == 0.0f && zw == 0.0f )
				xw = yw = zw = 1.0f;
			float vdist = Mathf.Sqrt(xw * xw + yw * yw + zw * zw);
			float mfac = size / vdist;

			dcy.x = Mathf.Exp(-FallOff.x * Mathf.Abs(xw));

			if ( !LinkFallOff )
			{
				dcy.y = Mathf.Exp(-FallOff.y * Mathf.Abs(yw));
				dcy.z = Mathf.Exp(-FallOff.z * Mathf.Abs(zw));
			}
			else
				dcy.y = dcy.z = dcy.x;

			p.x = cx + xw + (Mathf.Sign(xw) * ((Mathf.Abs(xw * mfac) - Mathf.Abs(xw)) * per.x) * dcy.x);
			p.y = cy + yw + (Mathf.Sign(yw) * ((Mathf.Abs(yw * mfac) - Mathf.Abs(yw)) * per.y) * dcy.y);
			p.z = cz + zw + (Mathf.Sign(zw) * ((Mathf.Abs(zw * mfac) - Mathf.Abs(zw)) * per.z) * dcy.z);
			return invtm.MultiplyPoint3x4(p);
		}

		public override void ModStart(MegaModifyObject mc)
		{
			xsize = bbox.max.x - bbox.min.x;
			ysize = bbox.max.y - bbox.min.y;
			zsize = bbox.max.z - bbox.min.z;
			size = (xsize > ysize) ? xsize : ysize;
			size = (zsize > size) ? zsize : size;
			size /= 2.0f;
			cx = bbox.center.x;
			cy = bbox.center.y;
			cz = bbox.center.z;

			// Get the percentage to spherify at this time
			per = Amount / 100.0f;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			xsize = bbox.max.x - bbox.min.x;
			ysize = bbox.max.y - bbox.min.y;
			zsize = bbox.max.z - bbox.min.z;
			size = (xsize > ysize) ? xsize : ysize;
			size = (zsize > size) ? zsize : size;
			size /= 2.0f;
			cx = bbox.center.x;
			cy = bbox.center.y;
			cz = bbox.center.z;

			// Get the percentage to spherify at this time
			per = Amount / 100.0f;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaBulge mod = root as MegaBulge;

			if ( mod )
			{
				Amount		= mod.Amount;
				FallOff		= mod.FallOff;
				LinkFallOff	= mod.LinkFallOff;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}