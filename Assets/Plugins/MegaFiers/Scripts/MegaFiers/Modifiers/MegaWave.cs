using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Wave")]
	public class MegaWave : MegaModifier
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
		public float	dir			= 0.0f;
		[Adjust]
		public bool		animate		= false;
		[Adjust]
		public float	Speed		= 1.0f;
		public int		divs		= 4;
		public int		numSegs		= 4;
		public int		numSides	= 4;
		float			time		= 0.0f;
		float			dy			= 0.0f;
		float			dist		= 0.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Wave"; }
		public override string GetHelpURL() { return "?page_id=357"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				dist;
			public float				flex;
			public float				time;
			public float				amp;
			public float				amp2;
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

				float u = math.abs(2.0f * p.x / dist);
				u = u * u;
				p.z += flex * WaveFunc(p.y, time, amp * (1.0f - u) + amp2 * u, wave, phase, dy);

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.dist		= dist;
				job.flex		= flex;
				job.time		= time;
				job.amp			= amp;
				job.amp2		= amp2;
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

			float u = Mathf.Abs(2.0f * p.x / dist);
			u = u * u;
			p.z += flex * MegaUtils.WaveFunc(p.y, time, amp * (1.0f - u) + amp2 * u, wave, phase, dy);
			return invtm.MultiplyPoint3x4(p);
		}

		float t = 0.0f;

		Matrix4x4 mat = new Matrix4x4();

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( animate )
			{
				if ( Application.isPlaying )
					t += Time.deltaTime * Speed;
				phase = t;
			}

			mat = Matrix4x4.identity;

			MegaMatrix.RotateZ(ref mat, Mathf.Deg2Rad * dir);
			SetAxis(mat);

			dy = Decay / 1000.0f;

			dist = (wave / 10.0f) * 4.0f * 5.0f;

			if ( dist == 0.0f )
				dist = 1.0f;

			return true;
		}

	
		void BuildMesh(float t)
		{		
			Vector3 pos = Vector3.zero;
			Vector3 last = Vector3.zero;

			float Dy     = wave / (float)divs;
			float Dx     = Dy * 4;
			int den = (int)(Dx * numSides * 0.5f);
			float starty = -(float)numSegs / 2.0f * Dy;
			float startx = -(float)numSides / 2.0f * Dx;

			for ( int i = 0; i <= numSides; i++ )
			{
				pos.x   = startx + Dx * (float)i;
				float u   = Mathf.Abs(pos.x / ((den != 0) ? den : 0.00001f));
				u   = u * u;

				for ( int j = 0; j <= numSegs; j++ )
				{
					pos.y = starty + (float)j * Dy;
					pos.z = MegaUtils.WaveFunc(pos.y, t, amp * (1.0f - u) + amp2 * u, wave, phase, Decay / 1000.0f);

					if ( j > 0 )
						Gizmos.DrawLine(last, pos);

					last = pos;
				}
			}

			for ( int j = 0; j <= numSegs; j++ )
			{
				pos.y = starty + (float)j * Dy;

				for ( int i = 0; i <= numSides; i++ )
				{
					pos.x = startx + Dx * (float)i;
					float u   = Mathf.Abs(pos.x / ((den != 0) ? den : 0.00001f));
					u = u * u;
					pos.z = MegaUtils.WaveFunc(pos.y, t, amp * (1.0f - u) + amp2 * u, wave, phase, Decay / 1000.0f);

					if ( i > 0 )
						Gizmos.DrawLine(last, pos);

					last = pos;
				}
			}
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

			BuildMesh(t);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaWave mod = root as MegaWave;

			if ( mod )
			{
				amp			= mod.amp;
				amp2		= mod.amp2;
				flex		= mod.flex;
				wave		= mod.wave;
				phase		= mod.phase;
				Decay		= mod.Decay;
				dir			= mod.dir;
				animate		= false;
				Speed		= mod.Speed;
				divs		= mod.divs;
				numSegs		= mod.numSegs;
				numSides	= mod.numSides;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}