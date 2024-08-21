using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Sinus Curve")]
	public class MegaSinusCurveWarp : MegaWarp
	{
		public float		scale	= 1.0f;
		public float		wave	= 1.0f;
		public float		speed	= 1.0f;
		public float		phase	= 0.0f;
		public bool			animate	= false;
		Matrix4x4			mat		= new Matrix4x4();
		Job					job;
		JobHandle			jobHandle;

		public override string WarpName() { return "Sinus Curve"; }
		public override string GetHelpURL() { return "Bubble.htm"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				phase;
			public float				wave;
			public float				scale;
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
				float dcy = Mathf.Exp(-totaldecay * dist);

				p.y += math.sin(phase + (p.x * wave) + p.y + p.z) * scale;

				p = math.lerp(ip, p, dcy);

				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.phase		= phase;
				job.wave		= wave;
				job.scale		= scale;
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
			float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

			p.y += Mathf.Sin(phase + (p.x * wave) + p.y + p.z) * scale;

			p = Vector3.Lerp(ip, p, dcy);

			return invtm.MultiplyPoint3x4(p);
		}

		void Update()
		{
			if ( animate )
			{
				if ( Application.isPlaying )
					phase += Time.deltaTime * speed;
			}

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

			SetAxis(mat);
			return true;
		}
	}
}