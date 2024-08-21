using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Cylindrify")]
	public class MegaCylindrifyWarp : MegaWarp
	{
		public float	Percent = 0.0f;
		public MegaAxis axis;
		float			size1;
		float			per;
		Matrix4x4		mat = new Matrix4x4();
		Job				job;
		JobHandle		jobHandle;

		public override string WarpName() { return "Cylindrify"; }
		public override string GetHelpURL() { return "?page_id=166"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				size1;
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
				float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

				float k = ((size1 / Mathf.Sqrt(p.x * p.x + p.z * p.z) / 2.0f - 1.0f) * per * dcy) + 1.0f;
				p.x *= k;
				p.z *= k;

				p = Vector3.Lerp(ip, p, dcy);

				p = invtm.MultiplyPoint3x4(p);
				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.size1		= size1;
				job.per			= per;
				job.totaldecay	= totaldecay;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= mod.jverts;
				job.jsverts		= mod.jsverts;
				job.wtm			= mod.tm;
				job.winvtm		= mod.invtm;

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

			float k = ((size1 / Mathf.Sqrt(p.x * p.x + p.z * p.z) / 2.0f - 1.0f) * per * dcy) + 1.0f;
			p.x *= k;
			p.z *= k;

			p = Vector3.Lerp(ip, p, dcy);

			return invtm.MultiplyPoint3x4(p);
		}

		void Update()
		{
			Prepare(Decay);
		}

		public override bool Prepare(float decay)
		{
			totaldecay = Decay + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;

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

			float xsize = Width;
			float zsize = Length;
			size1 = (xsize > zsize) ? xsize : zsize;

			// Get the percentage to spherify at this time
			per = Percent / 100.0f;

			return true;
		}
	}
}