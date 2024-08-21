using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Hump")]
	public class MegaHumpWarp : MegaWarp
	{
		public float	amount	= 0.0f;
		public float	cycles	= 1.0f;
		public float	phase	= 0.0f;
		public bool		animate	= false;
		public float	speed	= 1.0f;
		public MegaAxis	axis	= MegaAxis.Z;
		float			amt;
		Vector3			size	= Vector3.zero;
		Job				job;
		JobHandle		jobHandle;

		public override string WarpName() { return "Hump"; }
		public override string GetHelpURL() { return "?page_id=207"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public MegaAxis				axis;
			public float				amt;
			public float3				size;
			public float				cycles;
			public float				phase;
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
				float dcy = math.exp(-totaldecay * math.abs(dist));

				switch ( axis )
				{
					case MegaAxis.X: p.x += amt * math.sin(math.sqrt(p.x * p.x / size.x) + math.sqrt(p.y * p.y / size.y) * math.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
					case MegaAxis.Y: p.y += amt * math.sin(math.sqrt(p.y * p.y / size.y) + math.sqrt(p.x * p.x / size.x) * math.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
					case MegaAxis.Z: p.z += amt * math.sin(math.sqrt(p.x * p.x / size.x) + math.sqrt(p.y * p.y / size.y) * math.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
				}

				p = math.lerp(ip, p, dcy);

				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.axis		= axis;
				job.amt			= amt;
				job.size		= size;
				job.cycles		= cycles;
				job.phase		= phase;
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

			switch ( axis )
			{
				case MegaAxis.X: p.x += amt * Mathf.Sin(Mathf.Sqrt(p.x * p.x / size.x) + Mathf.Sqrt(p.y * p.y / size.y) * Mathf.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
				case MegaAxis.Y: p.y += amt * Mathf.Sin(Mathf.Sqrt(p.y * p.y / size.y) + Mathf.Sqrt(p.x * p.x / size.x) * Mathf.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
				case MegaAxis.Z: p.z += amt * Mathf.Sin(Mathf.Sqrt(p.x * p.x / size.x) + Mathf.Sqrt(p.y * p.y / size.y) * Mathf.PI / 0.1f * (Mathf.Deg2Rad * cycles) + phase); break;
			}

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

			size.x = Width;
			size.y = Height;
			size.z = Length;

			amt = amount / 100.0f;

			return true;
		}
	}
}