using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Stretch")]
	public class MegaStretch : MegaModifier
	{
		[Adjust]
		public float	amount			= 0.0f;
		[Adjust]
		public bool		doRegion		= false;
		[Adjust]
		public float	to				= 0.0f;
		[Adjust]
		public float	from			= 0.0f;
		[Adjust]
		public float	amplify			= 0.0f;
		[Adjust]
		public MegaAxis	axis			= MegaAxis.X;
		float			heightMax		= 0.0f;
		float			heightMin		= 0.0f;
		float			amplifier		= 0.0f;
		Matrix4x4		mat				= new Matrix4x4();
		[Adjust]
		public bool		useheightaxis	= false;
		[Adjust]
		public MegaAxis	axis1			= MegaAxis.X;
		float			ovh				= 0.0f;
		Job				job;
		JobHandle		jobHandle;

		public override string ModName()	{ return "Stretch"; }
		public override string GetHelpURL() { return "?page_id=334"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float				amount;
			public bool					doRegion;
			public float				from;
			public float				to;
			public float				heightMax;
			public float				heightMin;
			public float				ovh;
			public float				amplifier;
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float normHeight;
				float xyScale, zScale;

				if ( amount == 0.0f || (heightMax - heightMin == 0) )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				if ( (doRegion) && (to - from == 0.0f) )
				{
					jsverts[vi] = jvertices[vi];
					return;
				}

				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				if ( doRegion && p.y > to )
					normHeight = (to - heightMin) * ovh;
				else if ( doRegion && p.y < from )
					normHeight = (from - heightMin) * ovh;
				else
					normHeight = (p.y - heightMin) * ovh;

				if ( amount < 0.0f )
				{
					xyScale = (amplifier * -amount + 1.0f);
					zScale = (-1.0f / (amount - 1.0f));
				}
				else
				{
					xyScale = 1.0f / (amplifier * amount + 1.0f);
					zScale = amount + 1.0f;
				}

				float a = 4.0f * (1.0f - xyScale);
				float fraction = (((a * normHeight) - a) * normHeight) + 1.0f;
				p.x *= fraction;
				p.z *= fraction;

				if ( doRegion && p.y < from )
					p.y += (zScale - 1.0f) * from;
				else if ( doRegion && p.y <= to )
					p.y *= zScale;
				else if ( doRegion && p.y > to )
					p.y += (zScale - 1.0f) * to;
				else
					p.y *= zScale;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.amount		= amount;
				job.doRegion	= doRegion;
				job.from		= from;
				job.to			= to;
				job.heightMax	= heightMax;
				job.heightMin	= heightMin;
				job.ovh			= ovh;
				job.amplifier	= amplifier;
				job.tm			= tm;
				job.invtm		= invtm;
				job.jvertices	= jverts;
				job.jsverts		= jsverts;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();
			}
		}

		void CalcBulge(MegaAxis axis, float stretch, float amplify)
		{
			amount = stretch;
			amplifier = (amplify >= 0.0f) ? amplify + 1.0f : 1.0f / (-amplify + 1.0f);

			if ( !doRegion )
			{
				MegaAxis ax = axis;

				switch ( ax )
				{
					case MegaAxis.X:
					heightMin = bbox.min.x;
					heightMax = bbox.max.x;
					break;

					case MegaAxis.Z:
					heightMin = bbox.min.y;
					heightMax = bbox.max.y;
					break;

					case MegaAxis.Y:
					heightMin = bbox.min.z;
					heightMax = bbox.max.z;
					break;
				}
			}
			else
			{
				heightMin = from;
				heightMax = to;
			}

			ovh = 1.0f / (heightMax - heightMin);
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			float normHeight;
			float xyScale, zScale;

			if ( amount == 0.0f || (heightMax - heightMin == 0) )
				return p;

			if ( (doRegion) && (to - from == 0.0f) )
				return p;

			p = tm.MultiplyPoint3x4(p);

			if ( doRegion && p.y > to )
				normHeight = (to - heightMin) * ovh;
			else if ( doRegion && p.y < from )
				normHeight = (from - heightMin) * ovh;
			else
				normHeight = (p.y - heightMin) * ovh;

			if ( amount < 0.0f )
			{
				xyScale = (amplifier * -amount + 1.0f);
				zScale = (-1.0f / (amount - 1.0f));
			}
			else
			{
				xyScale = 1.0f / (amplifier * amount + 1.0f);
				zScale = amount + 1.0f;
			}

			float a = 4.0f * (1.0f - xyScale);
			float fraction = (((a * normHeight) - a) * normHeight) + 1.0f;
			p.x *= fraction;
			p.z *= fraction;

			if ( doRegion && p.y < from )
				p.y += (zScale - 1.0f) * from;
			else if ( doRegion && p.y <= to )
				p.y *= zScale;
			else if ( doRegion && p.y > to )
				p.y += (zScale - 1.0f) * to;
			else
				p.y *= zScale;

			p = invtm.MultiplyPoint3x4(p);

			return p;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			mat = Matrix4x4.identity;

			switch ( axis )
			{
				case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
				case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
				case MegaAxis.Z: break;
			}

			SetAxis(mat);
			CalcBulge(axis, amount, amplify);
			return true;
		}

		public override void ExtraGizmo(MegaModContext mc)
		{
			if ( doRegion )
				DrawFromTo(axis, from, to, mc);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaStretch mod = root as MegaStretch;

			if ( mod )
			{
				amount			= mod.amount;
				doRegion		= mod.doRegion;
				to				= mod.to;
				from			= mod.from;
				amplify			= mod.amplify;
				axis			= mod.axis;
				useheightaxis	= mod.useheightaxis;
				axis1			= mod.axis1;
				gizmoPos		= mod.gizmoPos;
				gizmoRot		= mod.gizmoRot;
				gizmoScale		= mod.gizmoScale;
				bbox			= mod.bbox;
			}
		}
	}
}