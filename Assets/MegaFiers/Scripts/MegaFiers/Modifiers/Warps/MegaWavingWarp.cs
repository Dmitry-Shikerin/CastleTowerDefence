using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Waving")]
	public class MegaWavingWarp : MegaWarp
	{
		public float	amp			= 0.01f;
		public float	flex		= 1.0f;
		public float	wave		= 1.0f;
		public float	phase		= 0.0f;
		public bool		animate		= false;
		public float	Speed		= 1.0f;
		public MegaAxis	waveaxis	= MegaAxis.X;
		float			time		= 0.0f;
		float			dy			= 0.0f;
		int				ix			= 0;
		int				iz			= 2;
		float			t			= 0.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string WarpName()	{ return "Waving"; }
		public override string GetHelpURL() { return "?page_id=308"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public int					iz;
			public int					ix;
			public float				flex;
			public float				time;
			public float				amp;
			public float				wave;
			public float				phase;
			public float				dy;
			public float				totaldecay;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			wtm;
			public Matrix4x4			winvtm;

			float WaveFunc(float radius, float t, float amp, float waveLen, float phase, float decay)
			{
				float ang = math.PI * 2.0f * (radius / waveLen + phase);
				return amp * math.sin(ang) * math.exp(-decay * math.abs(radius));
			}

			public void Execute(int vi)
			{
				float3 p = wtm.MultiplyPoint3x4(jvertices[vi]);

				p = tm.MultiplyPoint3x4(p);

				float3 ip = p;
				float dist = math.length(p);
				float dcy = math.exp(-totaldecay * dist);

				float u = math.abs(2.0f * p[iz]);
				u = u * u;
				p[ix] += flex * WaveFunc(p[iz], time, amp * u, wave, phase, totaldecay);

				p = math.lerp(ip, p, dcy);

				p =  invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.ix			= ix;
				job.iz			= iz;
				job.flex		= flex;
				job.time		= time;
				job.amp			= amp;
				job.wave		= wave;
				job.phase		= phase;
				job.dy			= dy;
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

		static public float WaveFunc(float radius, float t, float amp, float waveLen, float phase, float decay)
		{
			float ang = Mathf.PI * 2.0f * (radius / waveLen + phase);
			return amp * Mathf.Sin(ang) * Mathf.Exp(-decay * Mathf.Abs(radius));
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			float u = Mathf.Abs(2.0f * p[iz]);
			u = u * u;
			p[ix] += flex * WaveFunc(p[iz], time, amp * u, wave, phase, totaldecay);
			return invtm.MultiplyPoint3x4(p);
		}

		void Update()
		{
			if ( animate )
			{
				float dt = Time.deltaTime;
				if ( dt == 0.0f )
					dt = 0.01f;
				if ( Application.isPlaying )
					t += dt * Speed;
				phase = t;
			}
		}

		public override bool Prepare(float decay)
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;

			if ( wave == 0.0f )
				wave = 0.000001f;

			dy = Decay / 1000.0f;

			totaldecay = dy + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;

			switch ( waveaxis )
			{
				case MegaAxis.X:
					ix = 0;
					iz = 2;
					break;

				case MegaAxis.Y:
					ix = 1;
					iz = 2;
					break;

				case MegaAxis.Z:
					ix = 2;
					iz = 0;
					break;
			}
			return true;
		}
	}
}