using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Spherify")]
	public class MegaSpherify : MegaModifier
	{
		[Adjust]
		public float	percent = 0.0f;
		[Adjust]
		public float	FallOff = 0.0f;
		float			per;
		float			xsize,ysize,zsize;
		float			size;
		float			cx,cy,cz;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName()	{ return "Spherify"; }
		public override string GetHelpURL() { return "?page_id=322"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				size;
			public float				FallOff;
			public float				per;
			public float				cx;
			public float				cy;
			public float				cz;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float xw, yw, zw;

				xw = p.x - cx; yw = p.y - cy; zw = p.z - cz;
				if ( xw == 0.0f && yw == 0.0f && zw == 0.0f )
					xw = yw = zw = 1.0f;
				float vdist = math.sqrt(xw * xw + yw * yw + zw * zw);
				float mfac = size / vdist;

				float dcy = math.exp(-FallOff * math.abs(vdist));

				p.x = cx + xw + (math.sign(xw) * ((math.abs(xw * mfac) - math.abs(xw)) * per) * dcy);
				p.y = cy + yw + (math.sign(yw) * ((math.abs(yw * mfac) - math.abs(yw)) * per) * dcy);
				p.z = cz + zw + (math.sign(zw) * ((math.abs(zw * mfac) - math.abs(zw)) * per) * dcy);

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.size		= size;
				job.FallOff		= FallOff;
				job.per			= per;
				job.cx			= cx;
				job.cy			= cy;
				job.cz			= cz;
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

			float dcy = Mathf.Exp(-FallOff * Mathf.Abs(vdist));

			p.x = cx + xw + (Mathf.Sign(xw) * ((Mathf.Abs(xw * mfac) - Mathf.Abs(xw)) * per) * dcy);
			p.y = cy + yw + (Mathf.Sign(yw) * ((Mathf.Abs(yw * mfac) - Mathf.Abs(yw)) * per) * dcy);
			p.z = cz + zw + (Mathf.Sign(zw) * ((Mathf.Abs(zw * mfac) - Mathf.Abs(zw)) * per) * dcy);
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

			per = percent / 100.0f;
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

			per = percent / 100.0f;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaSpherify mod = root as MegaSpherify;

			if ( mod )
			{
				percent		= mod.percent;
				FallOff		= mod.FallOff;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}