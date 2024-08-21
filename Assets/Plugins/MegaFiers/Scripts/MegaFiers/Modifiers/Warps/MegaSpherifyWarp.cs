using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Spherify")]
	public class MegaSpherifyWarp : MegaWarp
	{
		public float		percent = 0.0f;
		public float		FallOff = 0.0f;
		float				per;
		float				xsize,ysize,zsize;
		float				size;
		Job					job;
		JobHandle			jobHandle;

		public override string WarpName() { return "Spherify"; }
		public override string GetHelpURL() { return "?page_id=322"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				size;
			public float				FallOff;
			public float				per;
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
				float dcy1 = math.exp(-totaldecay * dist);

				float vdist = dist;
				float mfac = size / vdist;

				float dcy = math.exp(-FallOff * Mathf.Abs(vdist));

				p.x = p.x + (math.sign(p.x) * ((math.abs(p.x * mfac) - math.abs(p.x)) * per) * dcy);
				p.y = p.y + (math.sign(p.y) * ((math.abs(p.y * mfac) - math.abs(p.y)) * per) * dcy);
				p.z = p.z + (math.sign(p.z) * ((math.abs(p.z * mfac) - math.abs(p.z)) * per) * dcy);

				p = math.lerp(ip, p, dcy1);

				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.size		= size;
				job.FallOff		= FallOff;
				job.per			= per;
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
			p = tm.MultiplyPoint3x4(p);

			Vector3 ip = p;
			float dist = p.magnitude;
			float dcy1 = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

			float vdist = dist;
			float mfac = size / vdist;

			float dcy = Mathf.Exp(-FallOff * Mathf.Abs(vdist));

			p.x = p.x + (Mathf.Sign(p.x) * ((Mathf.Abs(p.x * mfac) - Mathf.Abs(p.x)) * per) * dcy);
			p.y = p.y + (Mathf.Sign(p.y) * ((Mathf.Abs(p.y * mfac) - Mathf.Abs(p.y)) * per) * dcy);
			p.z = p.z + (Mathf.Sign(p.z) * ((Mathf.Abs(p.z * mfac) - Mathf.Abs(p.z)) * per) * dcy);

			p = Vector3.Lerp(ip, p, dcy1);

			return invtm.MultiplyPoint3x4(p);
		}

		void Update()
		{
			Prepare(Decay);
		}

		public override bool Prepare(float decay)
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;

			totaldecay = Decay + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;

			xsize = Width;
			ysize = Height;
			zsize = Length;
			size = (xsize > ysize) ? xsize : ysize;
			size = (zsize > size) ? zsize : size;
			size /= 2.0f;

			// Get the percentage to spherify at this time
			per = percent / 100.0f;

			return true;
		}
	}
}