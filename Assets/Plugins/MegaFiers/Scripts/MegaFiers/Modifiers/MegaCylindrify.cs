using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Cylindrify")]
	public class MegaCylindrify : MegaModifier
	{
		[Adjust]
		public float	Percent = 0.0f;
		[Adjust]
		public float	Decay	= 0.0f;
		[Adjust]
		public MegaAxis axis;
		Matrix4x4		mat		= new Matrix4x4();
		Job				job;
		JobHandle		jobHandle;
		float			size;
		float			per;

		public override string ModName() { return "Cylindrify"; }
		public override string GetHelpURL() { return "?page_id=166"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;
			public float				decay;
			public float				size;
			public float				per;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int i)
			{
				Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);

				float dcy = math.exp(-decay * p.magnitude);

				float k = ((size / math.sqrt(p.x * p.x + p.z * p.z) / 2.0f - 1.0f) * per * dcy) + 1.0f;
				p.x *= k;
				p.z *= k;

				jsverts[i] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.decay		= Decay;
				job.size		= size;
				job.per			= per;
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

			float dcy = Mathf.Exp(-Decay * p.magnitude);

			float k = ((size / Mathf.Sqrt(p.x * p.x + p.z * p.z) / 2.0f - 1.0f) * per * dcy) + 1.0f;
			p.x *= k;
			p.z *= k;
			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public void SetTM1()
		{
			tm = Matrix4x4.identity;

			MegaMatrix.RotateZ(ref tm, -gizmoRot.z * Mathf.Deg2Rad);
			MegaMatrix.RotateY(ref tm, -gizmoRot.y * Mathf.Deg2Rad);
			MegaMatrix.RotateX(ref tm, -gizmoRot.x * Mathf.Deg2Rad);

			MegaMatrix.SetTrans(ref tm, gizmoPos + Offset);
			invtm = tm.inverse;
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

			float xsize = bbox.max.x - bbox.min.x;
			float zsize = bbox.max.z - bbox.min.z;
			size = (xsize > zsize) ? xsize : zsize;

			// Get the percentage to spherify at this time
			per = Percent / 100.0f;

			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaCylindrify mod = root as MegaCylindrify;

			if ( mod )
			{
				Percent		= mod.Percent;
				Decay		= mod.Decay;
				axis		= mod.axis;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}