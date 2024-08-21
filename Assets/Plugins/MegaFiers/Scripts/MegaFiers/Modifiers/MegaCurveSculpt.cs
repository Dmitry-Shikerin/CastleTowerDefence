using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Curve Sculpt")]
	public class MegaCurveSculpt : MegaModifier
	{
		[Adjust]
		public AnimationCurve	defCurveX		= new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 0.0f));
		[Adjust]
		public AnimationCurve	defCurveY		= new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 0.0f));
		[Adjust]
		public AnimationCurve	defCurveZ		= new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 0.0f));
		[Adjust]
		public AnimationCurve	defCurveSclX	= new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));
		[Adjust]
		public AnimationCurve	defCurveSclY	= new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));
		[Adjust]
		public AnimationCurve	defCurveSclZ	= new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));
		[Adjust]
		public Vector3			OffsetAmount	= Vector3.one;
		[Adjust]
		public Vector3			ScaleAmount		= Vector3.one;
		[Adjust]
		public MegaAxis			offsetX			= MegaAxis.X;
		[Adjust]
		public MegaAxis			offsetY			= MegaAxis.Y;
		[Adjust]
		public MegaAxis			offsetZ			= MegaAxis.Z;
		[Adjust]
		public MegaAxis			scaleX			= MegaAxis.X;
		[Adjust]
		public MegaAxis			scaleY			= MegaAxis.Y;
		[Adjust]
		public MegaAxis			scaleZ			= MegaAxis.Z;
		public bool				symX			= false;
		public bool				symY			= false;
		public bool				symZ			= false;
		Vector3					size			= Vector3.zero;
		Job						job;
		JobHandle				jobHandle;

		public override string ModName() { return "CurveSculpt"; }
		public override string GetHelpURL() { return "?page_id=655"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float3					min;
			public float3					size;
			public int						scaleX;
			public int						scaleY;
			public int						scaleZ;
			public int						offsetX;
			public int						offsetY;
			public int						offsetZ;
			public float3					ScaleAmount;
			public float3					OffsetAmount;
			[Unity.Collections.ReadOnly]
			public animationcurve			defCurveX;
			[Unity.Collections.ReadOnly]
			public animationcurve			defCurveY;
			[Unity.Collections.ReadOnly]
			public animationcurve			defCurveZ;
			[Unity.Collections.ReadOnly]
			public animationcurve			defCurveSclX;
			[Unity.Collections.ReadOnly]
			public animationcurve			defCurveSclY;
			[Unity.Collections.ReadOnly]
			public animationcurve			defCurveSclZ;
			public NativeArray<Vector3>		jvertices;
			public NativeArray<Vector3>		jsverts;
			public Matrix4x4				tm;
			public Matrix4x4				invtm;

			public void Execute(int vi)
			{
				Vector3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float alpha = 0.0f;

				alpha = (p.x - min.x) / size.x;
				p[(int)scaleX] *= defCurveSclX.Evaluate(alpha) * ScaleAmount.x;
				p[(int)offsetX] += defCurveX.Evaluate(alpha) * OffsetAmount.x;

				alpha = (p.y - min.y) / size.y;
				p[(int)scaleY] *= defCurveSclY.Evaluate(alpha) * ScaleAmount.y;
				p[(int)offsetY] += defCurveY.Evaluate(alpha) * OffsetAmount.y;

				alpha = (p.z - min.z) / size.z;
				p[(int)scaleZ] *= defCurveSclZ.Evaluate(alpha) * ScaleAmount.z;
				p[(int)offsetZ] += defCurveZ.Evaluate(alpha) * OffsetAmount.z;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.min					= bbox.min;
				job.size				= size;
				job.scaleX				= (int)scaleX;
				job.scaleY				= (int)scaleY;
				job.scaleZ				= (int)scaleZ;
				job.offsetX				= (int)offsetX;
				job.offsetY				= (int)offsetY;
				job.offsetZ				= (int)offsetZ;
				job.ScaleAmount			= ScaleAmount;
				job.OffsetAmount		= OffsetAmount;
				job.defCurveX.keys		= new NativeArray<Keyframe>(defCurveX.keys, Allocator.TempJob);
				job.defCurveY.keys		= new NativeArray<Keyframe>(defCurveY.keys, Allocator.TempJob);
				job.defCurveZ.keys		= new NativeArray<Keyframe>(defCurveZ.keys, Allocator.TempJob);
				job.defCurveSclX.keys	= new NativeArray<Keyframe>(defCurveSclX.keys, Allocator.TempJob);
				job.defCurveSclY.keys	= new NativeArray<Keyframe>(defCurveSclY.keys, Allocator.TempJob);
				job.defCurveSclZ.keys	= new NativeArray<Keyframe>(defCurveSclZ.keys, Allocator.TempJob);
				job.jvertices			= jverts;
				job.jsverts				= jsverts;
				job.tm					= tm;
				job.invtm				= invtm;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();

				job.defCurveX.keys.Dispose();
				job.defCurveY.keys.Dispose();
				job.defCurveZ.keys.Dispose();
				job.defCurveSclX.keys.Dispose();
				job.defCurveSclY.keys.Dispose();
				job.defCurveSclZ.keys.Dispose();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			float alpha = 0.0f;
			p = tm.MultiplyPoint3x4(p);

			alpha = (p.x - bbox.min.x) / size.x;
			p[(int)scaleX] *= defCurveSclX.Evaluate(alpha) * ScaleAmount.x;
			p[(int)offsetX] += defCurveX.Evaluate(alpha) * OffsetAmount.x;

			alpha = (p.y - bbox.min.y) / size.y;
			p[(int)scaleY] *= defCurveSclY.Evaluate(alpha) * ScaleAmount.y;
			p[(int)offsetY] += defCurveY.Evaluate(alpha) * OffsetAmount.y;

			alpha = (p.z - bbox.min.z) / size.z;
			p[(int)scaleZ] *= defCurveSclZ.Evaluate(alpha) * ScaleAmount.z;
			p[(int)offsetZ] += defCurveZ.Evaluate(alpha) * OffsetAmount.z;

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			size = bbox.max - bbox.min;
			return true;
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaCurveSculpt mod = root as MegaCurveSculpt;

			if ( mod )
			{
				defCurveX.keys		= mod.defCurveX.keys;
				defCurveY.keys		= mod.defCurveY.keys;
				defCurveZ.keys		= mod.defCurveZ.keys;
				defCurveSclX.keys	= mod.defCurveSclX.keys;
				defCurveSclY.keys	= mod.defCurveSclY.keys;
				defCurveSclZ.keys	= mod.defCurveSclZ.keys;
				OffsetAmount		= mod.OffsetAmount;
				ScaleAmount			= mod.ScaleAmount;
				offsetX				= mod.offsetX;
				offsetY				= mod.offsetY;
				offsetZ				= mod.offsetZ;
				scaleX				= mod.scaleX;
				scaleY				= mod.scaleY;
				scaleZ				= mod.scaleZ;
				symX				= mod.symX;
				symY				= mod.symY;
				symZ				= mod.symZ;
				gizmoPos			= mod.gizmoPos;
				gizmoRot			= mod.gizmoRot;
				gizmoScale			= mod.gizmoScale;
				bbox				= mod.bbox;
			}
		}
	}
}