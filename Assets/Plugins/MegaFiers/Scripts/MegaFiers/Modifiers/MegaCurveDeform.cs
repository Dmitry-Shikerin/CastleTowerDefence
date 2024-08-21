using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Curve Deform")]
	public class MegaCurveDeform : MegaModifier
	{
		[Adjust]
		public MegaAxis			axis			= MegaAxis.X;
		[Adjust]
		public AnimationCurve	defCurve		= new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 0.0f), new Keyframe(1.0f, 0.0f));
		[Adjust]
		public float			MaxDeviation	= 1.0f;
		float					width			= 0.0f;
		int						ax;
		[Adjust]
		public float			Pos				= 0.0f;
		[Adjust]
		public bool				UsePos			= false;
		Keyframe				key				= new Keyframe();
		Job						job;
		JobHandle				jobHandle;

		public override string ModName()	{ return "CurveDeform"; }
		public override string GetHelpURL() { return "?page_id=655"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float3				min;
			public float				width;
			public int					ax;
			public bool					UsePos;
			public float				MaxDeviation;
			public float				Pos;
			[Unity.Collections.ReadOnly]
			public animationcurve		defCurve;
			public NativeArray<Vector3>	jvertices;
			public NativeArray<Vector3>	jsverts;
			public Matrix4x4			tm;
			public Matrix4x4			invtm;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float alpha = (p[ax] - min[ax]) * width;
				if ( UsePos )
					alpha += Pos;

				p.y += defCurve.Evaluate(alpha) * MaxDeviation;

				jsverts[vi] = invtm.MultiplyPoint3x4(p);
			}
		}

		public override void Modify(MegaModifyObject mc)
		{
			if ( verts != null )
			{
				job.min				= bbox.min;
				job.width			= width;
				job.ax				= ax;
				job.UsePos			= UsePos;
				job.MaxDeviation	= MaxDeviation;
				job.Pos				= Pos;
				job.defCurve.keys	= new NativeArray<Keyframe>(defCurve.keys, Allocator.TempJob);
				job.jvertices		= jverts;
				job.jsverts			= jsverts;
				job.tm				= tm;
				job.invtm			= invtm;

				jobHandle = job.Schedule(jverts.Length, mc.batchCount);
				jobHandle.Complete();

				job.defCurve.keys.Dispose();
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			float alpha = (p[ax] - bbox.min[ax]) * width;
			if ( UsePos )
				alpha += Pos;

			p.y += defCurve.Evaluate(alpha) * MaxDeviation;

			return invtm.MultiplyPoint3x4(p);
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			ax = (int)axis;
			width = bbox.max[ax] - bbox.min[ax];
			if ( width != 0.0f )
				width = 1.0f / width;
			return true;
		}

		public float GetPos(float alpha)
		{
			float y = defCurve.Evaluate(alpha);
			return y;
		}

		public void SetKey(int index, float t, float v, float intan, float outtan)
		{
			key.time = t;
			key.value = v;
			key.inTangent = intan;
			key.outTangent = outtan;
			defCurve.MoveKey(index, key);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaCurveDeform mod = root as MegaCurveDeform;

			if ( mod )
			{
				defCurve.keys	= mod.defCurve.keys;
				axis			= mod.axis;
				MaxDeviation	= mod.MaxDeviation;
				Pos				= mod.Pos;
				UsePos			= mod.UsePos;
				gizmoPos		= mod.gizmoPos;
				gizmoRot		= mod.gizmoRot;
				gizmoScale		= mod.gizmoScale;
				bbox			= mod.bbox;
			}
		}
	}
}