using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Warps/Squeeze")]
	public class MegaSqueezeWarp : MegaWarp
	{
		public float			amount		= 0.0f;
		public float			crv			= 0.0f;
		public float			radialamount = 0.0f;
		public float			radialcrv	= 0.0f;
		public bool				doRegion	= false;
		public float			to			= 0.0f;
		public float			from		= 0.0f;
		public MegaAxis			axis		= MegaAxis.Y;
		Matrix4x4				mat			= new Matrix4x4();
		float k1;
		float k2;
		float k3;
		float k4;
		float l;
		float l2;
		float ovl;
		float ovl2;
		Job						job;
		JobHandle				jobHandle;

		void SetK(float K1, float K2, float K3, float K4)
		{
			k1 = K1;
			k2 = K2;
			k3 = K3;
			k4 = K4;
		}

		public override string WarpName() { return "Squeeze"; }
		public override string GetIcon() { return "MegaStretch icon.png"; }
		public override string GetHelpURL() { return "?page_id=338"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public bool					doRegion;
			public float				from;
			public float				to;
			public float				l;
			public float				l2;
			public float				ovl;
			public float				ovl2;
			public float				k1;
			public float				k2;
			public float				k3;
			public float				k4;
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

				float z;

				p = tm.MultiplyPoint3x4(p);

				float3 ip = p;
				float dist = math.length(p);
				float dcy = math.exp(-totaldecay * dist);

				if ( l != 0.0f )
				{
					if ( doRegion )
					{
						if ( p.y < from )
							z = from * ovl;
						else
						{
							if ( p.y > to )
								z = to * ovl;
							else
								z = p.y * ovl;
						}
					}
					else
						z = math.abs(p.y * ovl);


					float f = 1.0f + z * k1 + k2 * z * (1.0f - z);

					p.y *= f;
				}

				if ( l2 != 0.0f )
				{
					float dist1 = math.sqrt(p.x * p.x + p.z * p.z);
					float xy = dist1 * ovl2;
					float f1 = 1.0f + xy * k3 + k4 * xy * (1.0f - xy);
					p.x *= f1;
					p.z *= f1;
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
				job.doRegion	= doRegion;
				job.from		= from;
				job.to			= to;
				job.l			= l;
				job.l2			= l2;
				job.ovl			= ovl;
				job.ovl2		= ovl2;
				job.k1			= k1;
				job.k2			= k2;
				job.k3			= k3;
				job.k4			= k4;
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
			float z;

			p = tm.MultiplyPoint3x4(p);

			Vector3 ip = p;
			float dist = p.magnitude;
			float dcy = Mathf.Exp(-totaldecay * Mathf.Abs(dist));

			if ( l != 0.0f )
			{
				if ( doRegion )
				{
					if ( p.y < from )
						z = from * ovl;
					else
					{
						if ( p.y > to )
							z = to * ovl;
						else
							z = p.y * ovl;
					}
				}
				else
					z = Mathf.Abs(p.y * ovl);


				float f =  1.0f + z * k1 + k2 * z * (1.0f - z);

				p.y *= f;
			}

			if ( l2 != 0.0f )
			{
				float dist1 = Mathf.Sqrt(p.x * p.x + p.z * p.z);
				float xy = dist1 * ovl2;
				float f1 =  1.0f + xy * k3 + k4 * xy * (1.0f - xy);
				p.x *= f1;
				p.z *= f1;
			}

			p = Vector3.Lerp(ip, p, dcy);

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool Prepare(float decay)
		{
			tm = transform.worldToLocalMatrix;
			invtm = tm.inverse;
			mat = Matrix4x4.identity;
			SetAxis(mat);
			SetK(amount, crv, radialamount, radialcrv);
			Vector3 size = Vector3.zero;
			size.x = Width;
			size.y = Height;
			size.z = Length;

			switch ( axis )
			{
				case MegaAxis.X:
					l = size[0];
					l2 = Mathf.Sqrt(size[1] * size[1] + size[2] * size[2]);
					break;

				case MegaAxis.Y:
					l = size[1];
					l2 = Mathf.Sqrt(size[0] * size[0] + size[2] * size[2]);
					break;

				case MegaAxis.Z:
					l = size[2];
					l2 = Mathf.Sqrt(size[1] * size[1] + size[0] * size[0]);
					break;

			}

			if ( l != 0.0f )
				ovl = 1.0f / l;

			if ( l2 != 0.0f )
				ovl2 = 1.0f / l2;

			totaldecay = Decay + decay;
			if ( totaldecay < 0.0f )
				totaldecay = 0.0f;

			return true;
		}

		public override void ExtraGizmo()
		{
			if ( doRegion )
				DrawFromTo(MegaAxis.Z, from, to);
		}
	}
}