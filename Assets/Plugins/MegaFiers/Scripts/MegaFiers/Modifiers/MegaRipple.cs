using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Ripple")]
	public class MegaRipple : MegaModifier
	{
		[Adjust]
		public float	amp			= 0.0f;
		[Adjust]
		public float	amp2		= 0.0f;
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
		public float	radius		= 1.0f;
		public int		segments	= 10;
		public int		circles		= 4;
		float			time		= 0.0f;
		float			dy			= 0.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Ripple"; }
		public override string GetHelpURL() { return "?page_id=308"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				flex;
			public float				amp;
			public float				amp2;
			public float				time;
			public float				wave;
			public float				phase;
			public float				dy;

			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			float WaveFunc(float radius, float t, float amp, float waveLen, float phase, float decay)
			{
				if ( waveLen == 0.0f )
					waveLen = 0.0000001f;

				float ang = math.PI * 2.0f * (radius / waveLen + phase);
				return amp * math.sin(ang) * math.exp(-decay * math.abs(radius));
			}

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float a;

				if ( amp != amp2 )
				{
					float len = math.length(p);
					if ( len == 0.0f )
						a = amp;
					else
					{
						float u = (math.acos(p.x / len)) / math.PI;
						u = (u > 0.5f) ? (1.0f - u) : u;
						u *= 2.0f;
						u = math.smoothstep(0.0f, 1.0f, u);
						a = amp * (1.0f - u) + amp2 * u;
					}
				}
				else
					a = amp;

				float oldZ = p.y;
				p.y = 0.0f;
				float r = math.length(p);
				p.y = oldZ + flex * WaveFunc(r, time, a, wave, phase, dy);

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.flex		= flex;
				job.amp			= amp;
				job.amp2		= amp2;
				job.time		= time;
				job.wave		= wave;
				job.phase		= phase;
				job.dy			= dy;
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
			float a;
		
			if ( amp != amp2 )
			{
				float len  = p.magnitude;
				if ( len == 0.0f )
					a = amp;
				else
				{
					float u = (Mathf.Acos(p.x / len)) / Mathf.PI;
		 			u = (u > 0.5f) ? (1.0f - u) : u;
					u *= 2.0f;
		 			u = Mathf.SmoothStep(0.0f, 1.0f, u);
	 				a = amp * (1.0f - u) + amp2 * u;
				}
			}
			else
				a = amp;
	
			float oldZ = p.y;
			p.y = 0.0f;
			float r = p.magnitude;
			p.y = oldZ + flex * MegaUtils.WaveFunc(r, time, a, wave, phase, dy);
			return invtm.MultiplyPoint3x4(p);
		}

		float t = 0.0f;

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

			dy = Decay / 1000.0f;

			return true;
		}

		Vector3 GetPos(float u, float radius)
		{
			Vector3 pos = Vector3.zero;

			pos.x = radius * Mathf.Cos(u * Mathf.PI * 2.0f);
			pos.z = radius * Mathf.Sin(u * Mathf.PI * 2.0f);

			float u2 = (u > 0.5f) ? (u - 0.5f) : u;
			u2 = (u2 > 0.25f) ? (0.5f - u2) : u2;
			u2 = u2 * 4.0f;
			u2 = u2 * u2;
			pos.y = MegaUtils.WaveFunc(radius, t, amp * (1.0f - u2) + amp2 * u2, wave, phase, dy);

			return pos;
		}

		void MakeCircle(float t, float radius, float r1, float a1, float a2, float w, float s, float d, int numCircleSegs)
		{
			Vector3 last = Vector3.zero;
			Vector3 pos = Vector3.zero;
			Vector3 pos1 = Vector3.zero;
			Vector3 first = Vector3.zero;

			for ( int i = 0; i < numCircleSegs; i++ )
			{
				float u = (float)i / (float)numCircleSegs;
				pos = GetPos(u, radius);
				pos1 = GetPos(u, r1);

				if ( (i & 1) == 1 )
				{
					Gizmos.color = gizCol1;
				}
				else
					Gizmos.color = gizCol2;

				if ( i > 0 )
				{
					Gizmos.DrawLine(last, pos);
				}
				else
					first = pos;

				Gizmos.DrawLine(pos1, pos);

				last = pos;
			}

			Gizmos.DrawLine(last, first);
		}

		public override void DrawGizmo(MegaModContext context)
		{
			Gizmos.color = Color.yellow;

			Matrix4x4 gtm = Matrix4x4.identity;
			Vector3 pos = gizmoPos;
			pos.x = -pos.x;
			pos.y = -pos.y;
			pos.z = -pos.z;

			Vector3 scl = gizmoScale;
			scl.x = 1.0f - (scl.x - 1.0f);
			scl.y = 1.0f - (scl.y - 1.0f);
			gtm.SetTRS(pos, Quaternion.Euler(gizmoRot), scl);

			Gizmos.matrix = transform.localToWorldMatrix * gtm;

			float r1 = 0.0f;
			for ( int i = 0; i < circles; i++ )
			{
				float r = ((float)i / (float)circles) * radius;
				MakeCircle(t, r, r1, amp, amp2, wave, phase, dy, segments);
				r1 = r;
			}
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaRipple mod = root as MegaRipple;

			if ( mod )
			{
				amp			= mod.amp;
				amp2		= mod.amp2;
				flex		= mod.flex;
				wave		= mod.wave;
				phase		= mod.phase;
				Decay		= mod.Decay;
				animate		= false;
				Speed		= mod.Speed;
				radius		= mod.radius;
				segments	= mod.segments;
				circles		= mod.circles;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}