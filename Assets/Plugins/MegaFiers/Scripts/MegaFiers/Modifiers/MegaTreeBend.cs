using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Tree Bend")]
	public class MegaTreeBend : MegaModifier
	{
		[Adjust]
		public float	fBendScale	= 1.0f;
		[Adjust]
		public Vector3	vWind		= Vector3.zero;
		[Adjust]
		public float	WindDir		= 0.0f;
		[Adjust]
		public float	WindSpeed	= 0.0f;
		Vector2			waveIn		= Vector2.zero;
		Vector2			waves		= Vector2.zero;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName() { return "Tree Bend"; }
		public override string GetHelpURL() { return "?page_id=41"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				fBendScale;
			public float3				vWind;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float fLength = math.length(p);

				float fBF = p.y * fBendScale;
				fBF += 1.0f;
				fBF *= fBF;
				fBF = fBF * fBF - fBF;
				Vector3 vNewPos = p;
				vNewPos.x += vWind.x * fBF;
				vNewPos.z += vWind.y * fBF;
				p = vNewPos.normalized * fLength;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.fBendScale	= fBendScale;
				job.vWind		= vWind;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		float frac(float val)
		{
			int v = (int)val;
			return val - v;
		}

		Vector2 smoothCurve(Vector2 x)
		{
			x.x = x.x * x.x * (3.0f - 2.0f * x.x);
			x.y = x.y * x.y * (3.0f - 2.0f * x.y);
			return x;
		}

		Vector2 triangleWave(Vector2 x)
		{
			x.x = Mathf.Abs(frac(x.x + 0.5f) * 2.0f - 1.0f);
			x.y = Mathf.Abs(frac(x.y + 0.5f) * 2.0f - 1.0f);
			return x;
		}

		Vector2 smoothTriangleWave(Vector2 x)
		{
			return smoothCurve(triangleWave(x));
		}

		float windTurbulence(float bbPhase, float frequency, float strength)
		{
			waveIn.x = bbPhase + frequency;
			waveIn.y = bbPhase + frequency;

			waves.x = (frac(waveIn.x * 1.975f) * 2.0f - 1.0f);
			waves.y = (frac(waveIn.y * 0.793f) * 2.0f - 1.0f);
			waves = smoothTriangleWave(waves);

			return (waves.x + waves.y) * strength;
		}

		Vector2 windEffect(float bbPhase, Vector2 windDirection, float gustLength, float gustFrequency, float gustStrength, float turbFrequency, float turbStrength)
		{
			float turbulence = windTurbulence(bbPhase, turbFrequency, turbStrength);

			float gustPhase = Mathf.Clamp01(Mathf.Sin((bbPhase - gustFrequency) / gustLength));
			float gustOffset = (gustPhase * gustStrength) + ((0.2f + gustPhase) * turbulence);

			return gustOffset * windDirection;
		}

		public override void SetValues(MegaModifier mod)
		{
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			float fLength = p.magnitude;

			float fBF = p.y * fBendScale;
			fBF += 1.0f;
			fBF *= fBF;
			fBF = fBF * fBF - fBF;
			Vector3 vNewPos = p;
			vNewPos.x += vWind.x * fBF;
			vNewPos.z += vWind.y * fBF;
			p = vNewPos.normalized * fLength;  
			p = invtm.MultiplyPoint3x4(p);
			return p;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			vWind.x = Mathf.Sin(WindDir * Mathf.Deg2Rad) * WindSpeed;
			vWind.y = Mathf.Cos(WindDir * Mathf.Deg2Rad) * WindSpeed;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaTreeBend mod = root as MegaTreeBend;

			if ( mod )
			{
				fBendScale	= mod.fBendScale;
				vWind		= mod.vWind;
				WindDir		= mod.WindDir;
				WindSpeed	= mod.WindSpeed;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}