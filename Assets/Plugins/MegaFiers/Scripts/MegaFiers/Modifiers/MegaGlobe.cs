using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Globe")]
	public class MegaGlobe : MegaModifier
	{
		[Adjust]
		public float	dir			= -90.0f;
		[Adjust]
		public float	dir1		= -90.0f;
		[Adjust]
		public MegaAxis	axis		= MegaAxis.X;
		[Adjust]
		public MegaAxis	axis1		= MegaAxis.Z;
		Matrix4x4		mat			= new Matrix4x4();

		[Adjust]
		public bool		twoaxis		= true;
		Matrix4x4		tm1			= new Matrix4x4();
		Matrix4x4		invtm1		= new Matrix4x4();

		[Adjust]
		public float	r			= 0.0f;
		[Adjust]
		public float	r1			= 0.0f;
		[Adjust]
		public float	radius		= 10.0f;
		[Adjust]
		public bool		linkRadii	= true;
		[Adjust]
		public float	radius1		= 10.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName()	{ return "Globe"; }
		public override string GetHelpURL() { return "?page_id=3752"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public float				r;
			public float				r1;
			public bool					twoaxis;
			public Matrix4x4			tm;
			public Matrix4x4			tm1;
			public Matrix4x4			invtm;
			public Matrix4x4			invtm1;

			public void Execute(int vi)
			{
				if ( r == 0.0f )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				Vector3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float x = p.x;
				float y = p.y;

				float yr = (y / r);

				float c = math.cos(math.PI - yr);
				float s = math.sin(math.PI - yr);
				float px = r * c + r - x * c;
				p.x = px;
				float pz = r * s - x * s;
				p.y = pz;

				p = invtm.MultiplyPoint3x4(p);

				if ( twoaxis )
				{
					p = tm1.MultiplyPoint3x4(p);

					x = p.x;
					y = p.y;

					yr = (y / r1);

					c = math.cos(math.PI - yr);
					s = math.sin(math.PI - yr);
					px = r1 * c + r1 - x * c;
					p.x = px;
					pz = r1 * s - x * s;
					p.y = pz;

					p = invtm1.MultiplyPoint3x4(p);
				}

				jsverts[vi] = p;	//invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.r			= r;
				job.r1			= r1;
				job.twoaxis		= twoaxis;
				job.tm			= tm;
				job.invtm		= invtm;
				job.tm1			= tm1;
				job.invtm1		= invtm1;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		// Virtual method for all mods
		public override void SetValues(MegaModifier mod)
		{
			MegaBend bm = (MegaBend)mod;
			dir = bm.dir;
			axis = bm.axis;
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			if ( r == 0.0f )
				return p;

			p = tm.MultiplyPoint3x4(p);

			float x = p.x;
			float y = p.y;

			float yr = (y / r);

			float c = Mathf.Cos(Mathf.PI - yr);
			float s = Mathf.Sin(Mathf.PI - yr);
			float px = r * c + r - x * c;
			p.x = px;
			float pz = r * s - x * s;
			p.y = pz;

			p = invtm.MultiplyPoint3x4(p);

			if ( twoaxis )
			{
				p = tm1.MultiplyPoint3x4(p);

				x = p.x;
				y = p.y;

				yr = (y / r1);

				c = Mathf.Cos(Mathf.PI - yr);
				s = Mathf.Sin(Mathf.PI - yr);
				px = r1 * c + r1 - x * c;
				p.x = px;
				pz = r1 * s - x * s;
				p.y = pz;

				p = invtm1.MultiplyPoint3x4(p);
			}
			return p;
		}

		void Calc()
		{
			mat = Matrix4x4.identity;

			tm1 = tm;
			invtm1 = invtm;

			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir);
			SetAxis(mat);

			mat = Matrix4x4.identity;

			switch ( axis1 )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir1);
			Matrix4x4 itm = mat.inverse;
			tm1 = mat * tm1;
			invtm1 = invtm1 * itm;

			r = -radius;

			if ( linkRadii )
				r1 = -radius;
			else
				r1 = -radius1;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			Calc();
			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaGlobe mod = root as MegaGlobe;

			if ( mod )
			{
				radius		= mod.radius;
				linkRadii	= mod.linkRadii;
				radius1		= mod.radius1;
				dir			= mod.dir;
				axis		= mod.axis;
				twoaxis		= mod.twoaxis;
				dir1		= mod.dir1;
				axis1		= mod.axis1;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}