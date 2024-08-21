using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Noise")]
	public class MegaNoiseWarp : MegaWarp
	{
		public float	Scale		= 1.0f;
		public float	Freq		= 0.25f;
		public bool		Animate		= false;
		public float	Phase		= 0.0f;
		public Vector3	Strength	= new Vector3(0.0f, 0.0f, 0.0f);
		float			time		= 0.0f;
		float			scale;
		Vector3			n			= Vector3.zero;
		Vector3			d			= Vector3.zero;
		Vector3			sp			= Vector3.zero;
		Job				job;
		JobHandle		jobHandle;

		public override string WarpName() { return "Noise"; }
		public override string GetIcon() { return "MegaNoise icon.png"; }
		public override string GetHelpURL() { return "?page_id=2576"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float3				Strength;
			public float				time;
			public float				scale;
			public float				totaldecay;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			wtm;
			public Matrix4x4			winvtm;
			float3						sp;
			float3						d;
			float3						n;

			public void Execute(int vi)
			{
				float3 p = wtm.MultiplyPoint3x4(jvertices[vi]);

				p = tm.MultiplyPoint3x4(p);

				float3 ip = p;
				float dist = math.length(p);
				float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

				sp = p * scale + 0.5f;
				n.x = sp.y;
				n.y = sp.z;
				n.z = time;
				d.x = noise.snoise(n);

				n.x = sp.x;
				n.y = sp.z;
				n.z = time;
				d.y = noise.snoise(n);

				n.x = sp.x;
				n.y = sp.y;
				n.z = time;
				d.z = noise.snoise(n);

				p += d * Strength;  //math.mul(d, Strength);

				p.x = ip.x + ((p.x - ip.x) * dcy);
				p.y = ip.y + ((p.y - ip.y) * dcy);
				p.z = ip.z + ((p.z - ip.z) * dcy);

				p = invtm.MultiplyPoint3x4(p);

				jsverts[vi] = winvtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaWarpBind mod)
		{
			if ( mod.verts != null )
			{
				job.Strength	= Strength;
				job.time		= time;
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

			sp.x = p.x * scale + 0.5f;
			sp.y = p.y * scale + 0.5f;
			sp.z = p.z * scale + 0.5f;

			n.x = sp.y;
			n.y = sp.z;
			n.z = time;
			d.x = noise.snoise(n);

			n.x = sp.x;
			n.y = sp.z;
			n.z = time;
			d.y = noise.snoise(n);

			n.x = sp.x;
			n.y = sp.y;
			n.z = time;
			d.z = noise.snoise(n);

			p.x += d.x * Strength.x;
			p.y += d.y * Strength.y;
			p.z += d.z * Strength.z;

			p.x = ip.x + ((p.x - ip.x) * dcy);
			p.y = ip.y + ((p.y - ip.y) * dcy);
			p.z = ip.z + ((p.z - ip.z) * dcy);

			return invtm.MultiplyPoint3x4(p);
		}

		void Update()
		{
			if ( Animate )
			{
				if ( Application.isPlaying )
					Phase += Time.deltaTime * Freq;
			}
			time = Phase;
		}

		public override bool Prepare(float decay)
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;

			if ( Scale == 0.0f )
				scale = 0.000001f;
			else
				scale = 1.0f / Scale;

			totaldecay = Decay + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;

			return true;
		}
	}
}