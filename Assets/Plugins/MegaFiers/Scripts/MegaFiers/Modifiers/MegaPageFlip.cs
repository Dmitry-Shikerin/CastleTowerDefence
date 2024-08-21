using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Page Flip")]
	public class MegaPageFlip : MegaModifier
	{
		[Adjust]
		public bool		animT		= false;
		[Adjust]
		public bool		autoMode	= true;
		[Adjust]
		public bool		lockRho		= true;
		[Adjust]
		public bool		lockTheta	= true;
		[Adjust]
		public float	timeStep	= 0.01f;
		[Adjust]
		public float	rho			= 0.0f;
		[Adjust]
		public float	theta		= 0.0f;
		[Adjust]
		public float	deltaT		= 0.0f;
		[Adjust]
		public float	kT			= 1.0f;
		[Adjust]
		public float	turn		= 0.0f;
		[Adjust]
		public float	ap1			= -15.0f;
		[Adjust]
		public float	ap2			= -2.5f;
		[Adjust]
		public float	ap3			= -3.5f;
		[Adjust]
		public bool		flipx		= true;
		Vector3			apex		= new Vector3(0.0f, 0.0f, -3.0f);
		Vector3			_cornerP;
		Vector3			_pageOrigin;
		float			fx			= 1.0f;
		Job				job;
		JobHandle		jobHandle;

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				fx;
			public float				theta;
			public float3				apex;
			public float				rho;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;


			public float3 curlTurn(float3 p)
			{
				float rhs = math.sqrt((p.x * p.x) + math.pow((p.z - apex.z), 2.0f));
				float num2 = rhs * math.sin(theta);
				float f = math.asin(p.x / rhs) / math.sin(theta);
				p.x = num2 * math.sin(f);
				p.z = (rhs + apex.z) - ((num2 * (1.0f - math.cos(f))) * math.sin(theta));
				p.y = (num2 * (1.0f - math.cos(f))) * math.cos(theta);
				return p;
			}

			public float3 rotpage(float3 p)
			{
				float x = p.x;
				float y = p.y;
				p.x = math.cos(rho * Mathf.Deg2Rad) * x + math.sin(rho * Mathf.Deg2Rad) * y;
				p.y = math.sin(rho * Mathf.Deg2Rad) * -x + math.cos(rho * Mathf.Deg2Rad) * y;
				return p;
			}

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				p = curlTurn(p);
				p.x *= fx;
				p = rotpage(p);
				p.x *= fx;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.fx			= fx;
				job.theta		= theta;
				job.rho			= rho;
				job.apex		= apex;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public void calcAuto(float t)
		{
			float num = 90.0f * Mathf.Deg2Rad;
			if ( t == 0.0f )
			{
				rho = 0.0f;
				theta = num;
				apex.z = ap1;
			}
			else
			{
				float num2;
				float num3;
				float num4;
				if ( t <= 0.15f )
				{
					num2 = t / 0.15f;
					num3 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 0.05f)) / 2.0f);
					num4 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 0.5f)) / 2.0f);
					rho = t * 180.0f;
					theta = funcLinear(num3, 90.0f * Mathf.Deg2Rad, 8.0f * Mathf.Deg2Rad);
					apex.z = funcLinear(num4, ap1, ap2);
				}
				else
				{
					if ( t <= 0.4f )
					{
						num2 = (t - 0.15f) / 0.25f;
						rho = t * 180f;
						theta = funcLinear(num2, 8.0f * Mathf.Deg2Rad, 6.0f * Mathf.Deg2Rad);
						apex.z = funcLinear(num2, ap2, ap3);
					}
					else
					{
						if ( t <= 1.0f )
						{
							num2 = (t - 0.4f) / 0.6f;
							rho = t * 180.0f;
							num3 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 10.0f)) / 2.0f);
							num4 = Mathf.Sin((Mathf.PI * Mathf.Pow(num2, 2.0f)) / 2.0f);
							theta = funcLinear(num3, 6.0f * Mathf.Deg2Rad, 90.0f * Mathf.Deg2Rad);
							apex.z = funcLinear(num4, ap3, ap1);
						}
					}
				}
			}
		}

		public float calcTheta(float _rho)
		{
			int num = 0;
			float num2 = 1.0f;
			float num3 = 0.05f;
			float num4 = 90.0f * Mathf.Deg2Rad;
			float num5 = (num2 - num3) * num4;
			float num6 = _rho / 180.0f;
			if ( num6 < 0.25f )
				num = (int)(num6 / 0.25f);
			else
			{
				if ( num6 < 0.5f )
					num = 1;
				else
				{
					if ( num6 <= 1.0f )
						num = (int)((1.0f - num6) * 0.5f);
				}
			}

			return (num4 - (num * num5));
		}

		public float calcTheta2(float t)
		{
			float num = 0.1f;
			float num2 = 45.0f * Mathf.Deg2Rad;
			float num3 = Mathf.Abs(1.0f - (t * 2.0f));
			return ((num * num2) + (num3 * num2));
		}

		public Vector3 curlTurn(Vector3 p)
		{
			float rhs = Mathf.Sqrt((p.x * p.x) + Mathf.Pow((p.z - apex.z), 2.0f));
			float num2 = rhs * Mathf.Sin(theta);
			float f = Mathf.Asin(p.x / rhs) / Mathf.Sin(theta);
			p.x = num2 * Mathf.Sin(f);
			p.z = (rhs + apex.z) - ((num2 * (1.0f - Mathf.Cos(f))) * Mathf.Sin(theta));
			p.y = (num2 * (1.0f - Mathf.Cos(f))) * Mathf.Cos(theta);
			return p;
		}

		public float funcLinear(float ft, float f0, float f1)
		{
			return (f0 + ((f1 - f0) * ft));
		}

		public float funcQuad(float ft, float f0, float f1, float p)
		{
			return (f0 + ((f1 - f0) * Mathf.Pow(ft, p)));
		}

		public override string ModName() { return "PageFlip"; }
		public override string GetHelpURL() { return "?page_id=271"; }

		public Vector3 flatTurn1(Vector3 p)
		{
			float rhs = p.x;
			p.x = Mathf.Cos(rho * Mathf.Deg2Rad) * rhs;
			p.y = Mathf.Sin(rho * Mathf.Deg2Rad) * -rhs;
			return p;
		}

		public Vector3 rotpage(Vector3 p)
		{
			float x = p.x;
			float y = p.y;
			p.x = Mathf.Cos(rho * Mathf.Deg2Rad) * x + Mathf.Sin(rho * Mathf.Deg2Rad) * y;
			p.y = Mathf.Sin(rho * Mathf.Deg2Rad) * -x + Mathf.Cos(rho * Mathf.Deg2Rad) * y;
			return p;
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);
			p = curlTurn(p);
			p.x *= fx;
			p = rotpage(p);
			p.x *= fx;
			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( flipx )
				fx = -1.0f;
			else
				fx = 1.0f;

			theta = 15.0f * Mathf.Deg2Rad;

			if ( turn < 0.0f )
				turn = 0.0f;

			if ( turn > 100.0f )
				turn = 100.0f;

			deltaT = turn / 100.0f;

			if ( animT )
				deltaT = (kT * Time.time) % 1.0f;
			if ( autoMode )
				calcAuto(deltaT);

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaPageFlip mod = root as MegaPageFlip;

			if ( mod )
			{
				animT		= mod.animT;
				autoMode	= mod.autoMode;
				lockRho		= mod.lockRho;
				lockTheta	= mod.lockTheta;
				timeStep	= mod.timeStep;
				rho			= mod.rho;
				theta		= mod.theta;
				deltaT		= mod.deltaT;
				kT			= mod.kT;
				turn		= mod.turn;
				ap1			= mod.ap1;
				ap2			= mod.ap2;
				ap3			= mod.ap3;
				flipx		= mod.flipx;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}