using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Squeeze")]
	public class MegaSqueeze : MegaModifier
	{
		[Adjust]
		public float	amount			= 0.0f;
		[Adjust]
		public float	crv				= 0.0f;
		[Adjust]
		public float	radialamount	= 0.0f;
		[Adjust]
		public float	radialcrv		= 0.0f;
		[Adjust]
		public bool		doRegion		= false;
		[Adjust]
		public float	to				= 0.0f;
		[Adjust]
		public float	from			= 0.0f;
		[Adjust]
		public MegaAxis	axis			= MegaAxis.Y;
		Matrix4x4		mat				= new Matrix4x4();
		float			k1;
		float			k2;
		float			k3;
		float			k4;
		float			l;
		float			l2;
		float			ovl;
		float			ovl2;
		Job				job;
		JobHandle		jobHandle;

		void SetK(float K1, float K2, float K3, float K4)
		{
			k1 = K1;
			k2 = K2;
			k3 = K3;
			k4 = K4;
		}

		public override string ModName() { return "Squeeze"; }
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
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float z;

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
					float dist = math.sqrt(p.x * p.x + p.z * p.z);
					float xy = dist * ovl2;
					float f1 = 1.0f + xy * k3 + k4 * xy * (1.0f - xy);
					p.x *= f1;
					p.z *= f1;
				}

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
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
			float z;

			p = tm.MultiplyPoint3x4(p);

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
				float dist = Mathf.Sqrt(p.x * p.x + p.z * p.z);
				float xy = dist * ovl2;
				float f1 =  1.0f + xy * k3 + k4 * xy * (1.0f - xy);
				p.x *= f1;
				p.z *= f1;
			}

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			mat = Matrix4x4.identity;

			SetAxis(mat);
			SetK(amount, crv, radialamount, radialcrv);
			Vector3 size = bbox.Size();

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
			return true;
		}

		public override void ExtraGizmo(MegaModContext mc)
		{
			if ( doRegion )
				DrawFromTo(MegaAxis.Z, from, to, mc);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaSqueeze mod = root as MegaSqueeze;

			if ( mod )
			{
				amount			= mod.amount;
				crv				= mod.crv;
				radialamount	= mod.radialamount;
				radialcrv		= mod.radialcrv;
				doRegion		= mod.doRegion;
				to				= mod.to;
				from			= mod.from;
				axis			= mod.axis;
				gizmoPos		= mod.gizmoPos;
				gizmoRot		= mod.gizmoRot;
				gizmoScale		= mod.gizmoScale;
				bbox			= mod.bbox;
			}
		}
	}
}