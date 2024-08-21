using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Bend")]
	public class MegaBend : MegaModifier
	{
		[Adjust]
		public bool			useRadius	= false;
		[Adjust]
		public float		radius		= 10.0f;
		[Adjust("Angle")]
		public float		angle		= 0.0f;
		[Adjust]
		public float		dir			= 0.0f;
		[Adjust]
		public MegaAxis		axis		= MegaAxis.Z;
		[Adjust]
		public bool			doRegion	= false;
		[Adjust]
		public float		from		= 0.0f;
		[Adjust]
		public float		to			= 0.0f;
		Matrix4x4			mat			= new Matrix4x4();
		public Matrix4x4	tmAbove		= new Matrix4x4();
		public Matrix4x4	tmBelow		= new Matrix4x4();
		float				r			= 0.0f;
		float				oor			= 0.0f;
		BendJob				bendJob;
		JobHandle			jobHandle;

		public override string ModName()	{ return "Bend"; }
		public override string GetHelpURL() { return "?page_id=41"; }

		[BurstCompile]
		struct BendJob : IJobParallelFor
		{
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	jvertices;
			public NativeArray<Vector3>	jsverts;
			public float				angle;
			public float				dir;
			public bool					doRegion;
			public float				from;
			public float				to;
			public float				r;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;
			public Matrix4x4			tmBelow;
			public Matrix4x4			tmAbove;
			public float				oor;

			public void Execute(int i)
			{
				if ( r == 0.0f && !doRegion )
					jsverts[i] = jvertices[i];
				else
				{
					Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

					if ( doRegion )
					{
						if ( p.y <= from )
							jsverts[i] = invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
						else
						{
							if ( p.y >= to )
								jsverts[i] = invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
							else
							{
								if ( r == 0.0f )
									jsverts[i] = invtm.MultiplyPoint3x4(p);
								else
								{
									float x = p.x;
									float y = p.y;
									float yr = 3.14159274f - (y * oor);

									float c = math.cos(yr);
									float s = math.sin(yr);
									p.x = r * c + r - x * c;
									p.y = r * s - x * s;
									jsverts[i] = invtm.MultiplyPoint3x4(p);
								}
							}
						}
					}
					else
					{
						if ( r == 0.0f )
							jsverts[i] = invtm.MultiplyPoint3x4(p);
						else
						{
							float x = p.x;
							float y = p.y;
							float yr = 3.14159274f - (y * oor);

							float c = math.cos(yr);
							float s = math.sin(yr);
							p.x = r * c + r - x * c;
							p.y = r * s - x * s;
							jsverts[i] = invtm.MultiplyPoint3x4(p);
						}
					}
				}
			}
		}

		public override void SetValues(MegaModifier mod)
		{
			MegaBend bm = (MegaBend)mod;
			angle		= bm.angle;
			dir			= bm.dir;
			axis		= bm.axis;
			doRegion	= bm.doRegion;
			from		= bm.from;
			to			= bm.to;
		}

		void CalcR(MegaAxis axis, float ang)
		{
			if ( useRadius )
				r = radius;
			else
			{
				float len = 0.0f;

				if ( !doRegion )
				{
					switch ( axis )
					{
						case MegaAxis.X: len = bbox.max.x - bbox.min.x; break;
						case MegaAxis.Z: len = bbox.max.y - bbox.min.y; break;
						case MegaAxis.Y: len = bbox.max.z - bbox.min.z; break;
					}
				}
				else
					len = to - from;

				if ( Mathf.Abs(ang) < 0.000001f )
					r = 0.0f;
				else
					r = len / ang;
			}

			oor = 1.0f / r;
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				bendJob.oor			= this.oor;
				bendJob.tmAbove		= this.tmAbove;
				bendJob.tmBelow		= this.tmBelow;
				bendJob.tm			= this.tm;
				bendJob.invtm		= this.invtm;
				bendJob.r			= this.r;
				bendJob.angle		= this.angle;
				bendJob.dir			= this.dir;
				bendJob.doRegion	= this.doRegion;
				bendJob.from		= this.from;
				bendJob.to			= this.to;
				bendJob.jvertices	= jverts;
				bendJob.jsverts		= jsverts;

				jobHandle = bendJob.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			if ( r == 0.0f && !doRegion )
				return p;

			p = tm.MultiplyPoint3x4(p);

			if ( doRegion )
			{
				if ( p.y <= from )
					return invtm.MultiplyPoint3x4(tmBelow.MultiplyPoint3x4(p));
				else
				{
					if ( p.y >= to )
						return invtm.MultiplyPoint3x4(tmAbove.MultiplyPoint3x4(p));
				}
			}

			if ( r == 0.0f )
				return invtm.MultiplyPoint3x4(p);

			float x = p.x;
			float y = p.y;
			float yr = Mathf.PI - (y * oor);

			float c = math.cos(yr);
			float s = math.sin(yr);
			p.x = r * c + r - x * c;
			p.y = r * s - x * s;
			return invtm.MultiplyPoint3x4(p);
		}

		public void Calc()
		{
			if ( from > to)	from = to;
			if ( to < from ) to = from;

			mat = Matrix4x4.identity;

			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			MegaMatrix.RotateY(ref mat, Mathf.Deg2Rad * dir);
			SetAxis(mat);

			CalcR(axis, Mathf.Deg2Rad * -angle);

			if ( doRegion )
			{
				doRegion = false;
				float len  = to - from;
				float rat1, rat2;

				if ( len == 0.0f )
					rat1 = rat2 = 1.0f;
				else
				{
					rat1 = to / len;
					rat2 = from / len;
				}

				Vector3 pt;
				tmAbove = Matrix4x4.identity;
				MegaMatrix.Translate(ref tmAbove, 0.0f, -to, 0.0f);
				MegaMatrix.RotateZ(ref tmAbove, -Mathf.Deg2Rad * angle * rat1);
				MegaMatrix.Translate(ref tmAbove, 0.0f, to, 0.0f);
				pt = new Vector3(0.0f, to, 0.0f);
				MegaMatrix.Translate(ref tmAbove, tm.MultiplyPoint3x4(Map(0, invtm.MultiplyPoint3x4(pt))) - pt);

				tmBelow = Matrix4x4.identity;
				MegaMatrix.Translate(ref tmBelow, 0.0f, -from, 0.0f);
				MegaMatrix.RotateZ(ref tmBelow, -Mathf.Deg2Rad * angle * rat2);
				MegaMatrix.Translate(ref tmBelow, 0.0f, from, 0.0f);
				pt = new Vector3(0.0f, from, 0.0f);
				MegaMatrix.Translate(ref tmBelow, tm.MultiplyPoint3x4(Map(0, invtm.MultiplyPoint3x4(pt))) - pt);

				doRegion = true;
			}
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

		public override void ExtraGizmo(MegaModContext mc)
		{
			if ( doRegion )
				DrawFromTo(axis, from, to, mc);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaBend bend = root as MegaBend;

			if ( bend )
			{
				useRadius	= bend.useRadius;
				radius		= bend.radius;
				angle		= bend.angle;
				axis		= bend.axis;
				dir			= bend.dir;
				doRegion	= bend.doRegion;
				from		= bend.from;
				to			= bend.to;
				gizmoPos	= bend.gizmoPos;
				gizmoRot	= bend.gizmoRot;
				gizmoScale	= bend.gizmoScale;
				bbox		= bend.bbox;
			}
		}
	}
}