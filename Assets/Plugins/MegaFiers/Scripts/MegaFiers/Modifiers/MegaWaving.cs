using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Waving")]
	public class MegaWaving : MegaModifier
	{
		[Adjust]
		public float	amp			= 0.01f;
		[Adjust]
		public float	flex		= 1.0f;
		[Adjust]
		public float	wave		= 1.0f;
		[Adjust]
		public float	phase		= 0.0f;
		[Adjust]
		public float	Decay		= 0.0f;
		[Adjust]
		public bool		animate		= false;
		[Adjust]
		public float	Speed		= 1.0f;
		[Adjust]
		public MegaAxis	waveaxis	= MegaAxis.X;
		[Adjust]
		public bool		useCurve	= false;
		[Adjust]
		public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		//[Adjust]
		//public MegaAxis curveaxis	= MegaAxis.Y;

		float time		= 0.0f;
		float			dy			= 0.0f;
		int				ix			= 0;
		int				iz			= 2;
		float			t			= 0.0f;
		Job				job;
		JobHandle		jobHandle;
		public Vector3 min;
		public Vector3 oomax;

		public override string ModName() { return "Waving"; }
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
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public bool					usecurve;
			[Unity.Collections.ReadOnly]
			public animationcurve		crv;
			//public int					caxis;
			public Vector3				min;
			public Vector3				oomax;

			float WaveFunc(float radius, float t, float amp, float waveLen, float phase, float decay)
			{
				float ang = math.PI * 2.0f * (radius / waveLen + phase);
				return amp * math.sin(ang) * math.exp(-decay * math.abs(radius));
			}

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float u = math.abs(2.0f * p[iz]);
				u = u * u;

				if ( usecurve )
				{
					float a = (p[iz] - min[iz]) * oomax[iz];
					p[ix] += flex * WaveFunc(p[iz], time, amp * u, wave, phase, dy) * crv.Evaluate(a);
				}
				else
				{
					p[ix] += flex * WaveFunc(p[iz], time, amp * u, wave, phase, dy);
				}

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.ix			= ix;
				job.iz			= iz;
				job.flex		= flex;
				job.time		= time;
				job.amp			= amp;
				job.wave		= wave;
				job.phase		= phase;
				job.dy			= dy;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;
				job.min			= min;
				job.oomax		= oomax;
				job.usecurve	= useCurve;

				//if ( useCurve )
					job.crv.keys = new NativeArray<Keyframe>(curve.keys, Allocator.TempJob);    // only need to set once

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();

				if ( job.crv.keys.IsCreated )
					job.crv.keys.Dispose();
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
			if ( useCurve )
			{
				float a = (p[iz] - min[iz]) * oomax[iz];
				p[ix] += flex * WaveFunc(p[iz], time, amp * u, wave, phase, dy) * curve.Evaluate(a);
			}
			else
			{
				p[ix] += flex * WaveFunc(p[iz], time, amp * u, wave, phase, dy);
			}

			//p[ix] += flex * WaveFunc(p[iz], time, amp * u, wave, phase, dy);
			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
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

			if ( wave == 0.0f )
				wave = 0.000001f;

			dy = Decay / 1000.0f;

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

			min = mc.bbox.min;
			Vector3 size = mc.bbox.Size();
			oomax = new Vector3(1.0f / size.x, 1.0f / size.y, 1.0f / size.z);

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaWaving mod = root as MegaWaving;

			if ( mod )
			{
				amp			= mod.amp;
				flex		= mod.flex;
				wave		= mod.wave;
				phase		= mod.phase;
				Decay		= mod.Decay;
				animate		= false;
				Speed		= mod.Speed;
				waveaxis	= mod.waveaxis;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}