using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

using System.Runtime.CompilerServices;

namespace MegaFiers
{
	[AddComponentMenu("Modifiers/Twist")]
	public class MegaTwist : MegaModifier
	{
		[Adjust]
		public float	angle			= 0.0f;
		[Adjust]
		public bool		doRegion		= false;
		[Adjust]
		public float	from			= 0.0f;
		[Adjust]
		public float	to				= 0.0f;
		[Adjust]	//(-10.0f, 10.0f)]
		public float	Bias			= 0.0f;
		[Adjust]
		public MegaAxis	axis			= MegaAxis.Z;
		bool			doBias			= false;
		float			height			= 0.0f;
		float			angleOverHeight	= 0.0f;
		float			theAngle;
		float			bias;
		Matrix4x4		mat				= new Matrix4x4();
		TwistJob		twistJob;
		JobHandle		twistJobHandle;

		public override string ModName() { return "Twist"; }
		public override string GetHelpURL() { return "?page_id=341"; }

		public override void Modify(MegaModifyObject mc)
		{
			twistJob.angle		= angle;
			twistJob.doRegion	= doRegion;
			twistJob.from		= from;
			twistJob.to			= to;
			twistJob.bias		= bias;
			twistJob.axis		= axis;
			twistJob.doBias		= doBias;
			twistJob.height		= height;
			twistJob.angleOverHeight = angleOverHeight;
			twistJob.theAngle	= theAngle;
			twistJob.tm			= tm;
			twistJob.invtm		= invtm;
			twistJob.jvertices	= jverts;
			twistJob.jsverts	= jsverts;

			twistJob.tm1 = tm;
			twistJob.invtm1 = invtm;

			twistJobHandle = twistJob.Schedule(jverts.Length, mc.batchCount);
			twistJobHandle.Complete();
		}

		[BurstCompile]
		struct TwistJob : IJobParallelFor
		{
			public NativeArray<Vector3> jvertices;
			public NativeArray<Vector3> jsverts;

			public float		angle;
			public bool			doRegion;
			public float		from;
			public float		to;
			public MegaAxis		axis;
			public bool			doBias;
			public float		height;
			public float		angleOverHeight;
			public float		theAngle;
			public float		bias;
			public Matrix4x4	tm;
			public Matrix4x4	invtm;
			public float4x4 tm1;
			public float4x4 invtm1;

			 [MethodImpl(MethodImplOptions.AggressiveInlining)]
			public float3 MultiplyPoint3x4(float4x4 m, float3 point)
			{
				Vector3 result;
				result.x = m.c0.x * point.x + m.c0.y * point.y + m.c0.z * point.z + m.c0.w;
				result.y = m.c1.x * point.x + m.c1.y * point.y + m.c1.z * point.z + m.c1.w;
				result.z = m.c2.x * point.x + m.c2.y * point.y + m.c2.z * point.z + m.c2.w;
				return result;
			}

			public float3 MultiplyPoint3x41(float4x3 m, float4 p)
			{
				float3 result;
				float4 point = p;

				result.x = math.dot(m.c0, point) + m.c0.w;
				result.y = math.dot(m.c1, point) + m.c1.w;
				result.z = math.dot(m.c2, point) + m.c2.w;
				return result;
			}

			public void Execute(int i)
			{
				float z, a;
				if ( theAngle == 0.0f )
					jsverts[i] = jvertices[i];

				Vector3 p = tm.MultiplyPoint3x4(jvertices[i]);
				//float3 p = MultiplyPoint3x4(tm1, jvertices[i]);

				float x = p.x;
				float y = p.z;

				if ( doRegion )
				{
					if ( p.y < from )
						z = from;
					else
					{
						if ( p.y > to )
							z = to;
						else
							z = p.y;
					}
				}
				else
					z = p.y;

				if ( doBias )
				{
					float u = z / height;
					a = theAngle * (float)math.pow(math.abs(u), bias);
					if ( u < 0.0f )
						a = -a;
				}
				else
					a = z * angleOverHeight;

				float cosine = math.cos(Mathf.Deg2Rad * a);
				float sine = math.sin(Mathf.Deg2Rad * a);
				p.x = cosine * x + sine * y;
				p.z = -sine * x + cosine * y;

				jsverts[i] = invtm.MultiplyPoint3x4(p);
				//jsverts[i] = MultiplyPoint3x4(invtm, p);
			}
		}

		void CalcHeight(MegaAxis axis, float angle, MegaBox3 bbx)
		{
			switch ( axis )
			{
				case MegaAxis.X:	height = bbx.max.x - bbx.min.x;	break;
				case MegaAxis.Z:	height = bbx.max.y - bbx.min.y;	break;
				case MegaAxis.Y:	height = bbx.max.z - bbx.min.z;	break;
			}
	
			if ( height == 0.0f )
			{
				theAngle = 0.0f;
				angleOverHeight = 0.0f;
			}
			else
			{
				theAngle = angle;
				angleOverHeight = angle / height;
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			float z, a;
			if ( theAngle == 0.0f )
				return p;

			p = tm.MultiplyPoint3x4(p);

			float x = p.x;
			float y = p.z;

			if ( doRegion )
			{
				if ( p.y < from )
					z = from;
				else 
				{
					if ( p.y > to )
						z = to;
					else
						z = p.y;
				}
			}
			else
				z = p.y;

			if ( doBias )
			{
				float u = z / height;
				a = theAngle * (float)Mathf.Pow(Mathf.Abs(u), bias);
				if ( u < 0.0f )
					a = -a;
			}
			else
				a = z * angleOverHeight;

			float cosine = Mathf.Cos(Mathf.Deg2Rad * a);
			float sine = Mathf.Sin(Mathf.Deg2Rad * a);
			p.x =  cosine * x + sine * y;
			p.z = -sine * x + cosine * y;

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

			if ( Bias != 0.0f )
			{
				bias = 1.0f - (Bias + 100.0f) / 200.0f;
				if ( bias < 0.00001f )
					bias = 0.00001f;

				if ( bias > 0.99999f )
					bias = 0.99999f;

				bias = Mathf.Log(bias) / Mathf.Log(0.5f);
				doBias = true;
			}
			else
			{
				bias = 1.0f;
				doBias = false;
			}

			if ( from > to ) from = to;
			if ( to < from ) to = from;

			CalcHeight(axis, angle, mc.bbox);
			return true;
		}

		public override void ExtraGizmo(MegaModContext mc)
		{
			if ( doRegion )
				DrawFromTo(axis, from, to, mc);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaTwist mod = root as MegaTwist;

			if ( mod )
			{
				angle		= mod.angle;
				doRegion	= mod.doRegion;
				from		= mod.from;
				to			= mod.to;
				Bias		= mod.Bias;
				axis		= mod.axis;
				gizmoPos	= mod.gizmoPos;
				gizmoRot	= mod.gizmoRot;
				gizmoScale	= mod.gizmoScale;
				bbox		= mod.bbox;
			}
		}
	}
}