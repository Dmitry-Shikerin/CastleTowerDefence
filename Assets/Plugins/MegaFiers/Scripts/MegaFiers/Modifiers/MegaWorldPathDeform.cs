using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	public enum MegaLoopMode
	{
		Loop,
		Clamp,
		PingPong,
		None,
	}

	[AddComponentMenu("Modifiers/World Path Deform")]
	public class MegaWorldPathDeform : MegaModifier
	{
		[Adjust]
		public float			percent			= 0.0f;
		[Adjust]
		public float			stretch			= 1.0f;
		[Adjust]
		public float			twist			= 0.0f;
		[Adjust]
		public float			rotate			= 0.0f;
		[Adjust]
		public MegaAxis			axis			= MegaAxis.X;
		[Adjust]
		public bool				flip			= false;
		[Adjust]
		public MegaShape		path			= null;
		[Adjust]
		public bool				animate			= false;
		[Adjust]
		public float			speed			= 1.0f;
		public float			tangent			= 1.0f;
		[HideInInspector]
		public Matrix4x4		mat				= new Matrix4x4();
		public bool				UseTwistCurve	= false;
		public AnimationCurve	twistCurve		= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public bool				UseStretchCurve = false;
		public AnimationCurve	stretchCurve	= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public Vector3			Up				= Vector3.up;
		public int				curve			= 0;
		[Adjust]
		public bool				usedist			= false;
		[Adjust]
		public float			distance		= 0.0f;
		public MegaLoopMode		loopmode		= MegaLoopMode.None;
		float					usepercent;
		float					usetan;
		float					ovlen;
		Job						job;
		JobStretch				jobStretch;
		JobTwist				jobTwist;
		JobStretchTwist			jobStretchTwist;
		JobHandle				jobHandle;

		public override string ModName() { return "WorldPathDeform"; }
		public override string GetHelpURL() { return "?page_id=361"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			public float					twist;
			public float3					Up;
			public float					ovlen;
			public float					stretch;
			public float					usepercent;
			public float					usetan;
			public bool						normalizedInterp;
			public Matrix4x4				mat;
			public NativeArray<Vector3>		jvertices;
			public NativeArray<Vector3>		jsverts;
			public Matrix4x4				tm;
			public Matrix4x4				invtm;

			[Unity.Collections.ReadOnly]
			public megaspline				spline;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float alpha;
				float tws = 0.0f;

				alpha = (p.z * ovlen * stretch) + usepercent;

				if ( spline.closed )
					alpha = Mathf.Repeat(alpha, 1.0f);

				float3 ps = spline.InterpCurve3D(alpha, normalizedInterp, ref tws);
				float3 ps1 = spline.InterpCurve3D(alpha + usetan, normalizedInterp);

				quaternion tw = quaternion.identity;
				tw = quaternion.AxisAngle(new float3(0, 0, 1), Mathf.Deg2Rad * ((twist * alpha) + tws));

				ps1.x -= ps.x;
				ps1.y -= ps.y;
				ps1.z -= ps.z;
				ps1 = math.normalize(ps1);
				quaternion rotation = math.mul(quaternion.LookRotation(ps1, Up), tw);

				Matrix4x4 wtm = Matrix4x4.identity;
				MegaMatrix.SetTR(ref wtm, ps, rotation);
				wtm = mat * wtm;

				ps.x = (p.x * wtm[0]) + (p.y * wtm[4]) + wtm[12];
				ps.y = (p.x * wtm[1]) + (p.y * wtm[5]) + wtm[13];
				ps.z = (p.x * wtm[2]) + (p.y * wtm[6]) + wtm[14];

				jsverts[vi] = ps;	//invtm.MultiplyPoint3x4(p);
			}
		}

		[BurstCompile]
		struct JobStretch : IJobParallelFor
		{
			public float					twist;
			public float3					Up;
			public float					ovlen;
			public float					stretch;
			public float					usepercent;
			public float					usetan;
			public bool						normalizedInterp;
			public Matrix4x4				mat;
			[Unity.Collections.ReadOnly]
			public animationcurve			stretchCurve;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>		jvertices;
			public NativeArray<Vector3>		jsverts;
			public Matrix4x4				tm;
			public Matrix4x4				invtm;

			[Unity.Collections.ReadOnly]
			public megaspline				spline;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float alpha;
				float tws = 0.0f;

				float str = stretchCurve.Evaluate(Mathf.Repeat(p.z * ovlen + usepercent, 1.0f)) * stretch;
				alpha = (p.z * ovlen * str) + usepercent;

				if ( spline.closed )
					alpha = Mathf.Repeat(alpha, 1.0f);

				float3 ps = spline.InterpCurve3D(alpha, normalizedInterp, ref tws);
				float3 ps1 = spline.InterpCurve3D(alpha + usetan, normalizedInterp);

				quaternion tw = quaternion.identity;
				tw = quaternion.AxisAngle(new float3(0, 0, 1), Mathf.Deg2Rad * ((twist * alpha) + tws));

				ps1.x -= ps.x;
				ps1.y -= ps.y;
				ps1.z -= ps.z;
				ps1 = math.normalize(ps1);
				quaternion rotation = math.mul(quaternion.LookRotation(ps1, Up), tw);

				Matrix4x4 wtm = Matrix4x4.identity;
				MegaMatrix.SetTR(ref wtm, ps, rotation);
				wtm = mat * wtm;

				ps.x = (p.x * wtm[0]) + (p.y * wtm[4]) + wtm[12];
				ps.y = (p.x * wtm[1]) + (p.y * wtm[5]) + wtm[13];
				ps.z = (p.x * wtm[2]) + (p.y * wtm[6]) + wtm[14];

				jsverts[vi] = ps;	//invtm.MultiplyPoint3x4(p);
			}
		}

		[BurstCompile]
		struct JobTwist : IJobParallelFor
		{
			public float					twist;
			public float3					Up;
			public float					ovlen;
			public float					stretch;
			public float					usepercent;
			public float					usetan;
			public bool						normalizedInterp;
			public Matrix4x4				mat;
			[Unity.Collections.ReadOnly]
			public animationcurve			twistCurve;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>		jvertices;
			public NativeArray<Vector3>		jsverts;
			public Matrix4x4				tm;
			public Matrix4x4				invtm;

			[Unity.Collections.ReadOnly]
			public megaspline				spline;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float alpha;
				float tws = 0.0f;

				alpha = (p.z * ovlen * stretch) + usepercent;

				if ( spline.closed )
					alpha = Mathf.Repeat(alpha, 1.0f);

				float3 ps = spline.InterpCurve3D(alpha, normalizedInterp, ref tws);
				float3 ps1 = spline.InterpCurve3D(alpha + usetan, normalizedInterp);

				quaternion tw = quaternion.identity;
				float twst = twistCurve.Evaluate(alpha) * twist;
				tw = quaternion.AxisAngle(new float3(0, 0, 1), Mathf.Deg2Rad * (twst + tws));

				ps1.x -= ps.x;
				ps1.y -= ps.y;
				ps1.z -= ps.z;
				ps1 = math.normalize(ps1);
				quaternion rotation = math.mul(quaternion.LookRotation(ps1, Up), tw);

				Matrix4x4 wtm = Matrix4x4.identity;
				MegaMatrix.SetTR(ref wtm, ps, rotation);
				wtm = mat * wtm;

				ps.x = (p.x * wtm[0]) + (p.y * wtm[4]) + wtm[12];
				ps.y = (p.x * wtm[1]) + (p.y * wtm[5]) + wtm[13];
				ps.z = (p.x * wtm[2]) + (p.y * wtm[6]) + wtm[14];

				jsverts[vi] = ps;	//invtm.MultiplyPoint3x4(p);
			}
		}

		[BurstCompile]
		struct JobStretchTwist : IJobParallelFor
		{
			public float					twist;
			public float3					Up;
			public float					ovlen;
			public float					stretch;
			public float					usepercent;
			public float					usetan;
			public bool						normalizedInterp;
			public Matrix4x4				mat;
			[Unity.Collections.ReadOnly]
			public animationcurve			twistCurve;
			[Unity.Collections.ReadOnly]
			public animationcurve			stretchCurve;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>		jvertices;
			public NativeArray<Vector3>		jsverts;
			public Matrix4x4				tm;
			public Matrix4x4				invtm;

			[Unity.Collections.ReadOnly]
			public megaspline				spline;

			public void Execute(int vi)
			{
				float3 p = tm.MultiplyPoint3x4(jvertices[vi]);

				float alpha;
				float tws = 0.0f;

				float str = stretchCurve.Evaluate(Mathf.Repeat(p.z * ovlen + usepercent, 1.0f)) * stretch;
				alpha = (p.z * ovlen * str) + usepercent;

				if ( spline.closed )
					alpha = Mathf.Repeat(alpha, 1.0f);

				float3 ps = spline.InterpCurve3D(alpha, normalizedInterp, ref tws);
				float3 ps1 = spline.InterpCurve3D(alpha + usetan, normalizedInterp);

				quaternion tw = quaternion.identity;
				float twst = twistCurve.Evaluate(alpha) * twist;
				tw = quaternion.AxisAngle(new float3(0, 0, 1), Mathf.Deg2Rad * (twst + tws));

				ps1.x -= ps.x;
				ps1.y -= ps.y;
				ps1.z -= ps.z;
				ps1 = math.normalize(ps1);
				quaternion rotation = math.mul(quaternion.LookRotation(ps1, Up), tw);

				Matrix4x4 wtm = Matrix4x4.identity;
				MegaMatrix.SetTR(ref wtm, ps, rotation);
				wtm = mat * wtm;

				ps.x = (p.x * wtm[0]) + (p.y * wtm[4]) + wtm[12];
				ps.y = (p.x * wtm[1]) + (p.y * wtm[5]) + wtm[13];
				ps.z = (p.x * wtm[2]) + (p.y * wtm[6]) + wtm[14];

				jsverts[vi] = ps;	//invtm.MultiplyPoint3x4(p);
			}
		}

		// 23.8ms old way 0.48ms new way 50 times faster 5000% improvement
		public override void Modify(MegaModifyObject mc)
		{
			if ( UseStretchCurve )
			{
				if ( UseTwistCurve )
				{
					jobStretchTwist.spline.points = new NativeArray<splinepoint>(path.splines[curve].knots.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

					MegaSpline spline = path.splines[curve];
					splinepoint sp;

					for ( int i = 0; i < spline.knots.Count; i++ )
					{
						sp.p			= spline.knots[i].p;
						sp.invec		= spline.knots[i].invec;
						sp.outvec		= spline.knots[i].outvec;
						sp.length		= spline.knots[i].length;
						sp.twist		= spline.knots[i].twist;
						sp.seglength	= spline.knots[i].seglength;

						jobStretchTwist.spline.points[i] = sp;
					}

					jobStretchTwist.spline.closed		= spline.closed;
					jobStretchTwist.spline.length		= spline.length;
					jobStretchTwist.spline.twistmode	= spline.twistmode;

					if ( verts != null )
					{
						jobStretchTwist.stretchCurve.keys = new NativeArray<Keyframe>(stretchCurve.keys, Allocator.TempJob);    // only need to set once
						jobStretchTwist.twistCurve.keys = new NativeArray<Keyframe>(twistCurve.keys, Allocator.TempJob);

						jobStretchTwist.Up					= Up;
						jobStretchTwist.twist				= twist;
						jobStretchTwist.ovlen				= ovlen;
						jobStretchTwist.stretch				= stretch;
						jobStretchTwist.usepercent			= usepercent;
						jobStretchTwist.usetan				= usetan;
						jobStretchTwist.normalizedInterp	= path.normalizedInterp;
						jobStretchTwist.mat					= mat;
						jobStretchTwist.tm					= tm;
						jobStretchTwist.invtm				= invtm;
						jobStretchTwist.jvertices			= jverts;
						jobStretchTwist.jsverts				= jsverts;

						jobHandle = jobStretchTwist.Schedule(jverts.Length, mc.batchCount);
						jobHandle.Complete();

						if ( jobStretchTwist.stretchCurve.keys.IsCreated )
							jobStretchTwist.stretchCurve.keys.Dispose();

						if ( jobStretchTwist.twistCurve.keys.IsCreated )
							jobStretchTwist.twistCurve.keys.Dispose();
					}

					jobStretchTwist.spline.points.Dispose();    // only need to set once
				}
				else
				{
					jobStretch.spline.points = new NativeArray<splinepoint>(path.splines[curve].knots.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

					MegaSpline spline = path.splines[curve];
					splinepoint sp;

					for ( int i = 0; i < spline.knots.Count; i++ )
					{
						sp.p			= spline.knots[i].p;
						sp.invec		= spline.knots[i].invec;
						sp.outvec		= spline.knots[i].outvec;
						sp.length		= spline.knots[i].length;
						sp.twist		= spline.knots[i].twist;
						sp.seglength	= spline.knots[i].seglength;

						jobStretch.spline.points[i] = sp;
					}

					jobStretch.spline.closed = spline.closed;
					jobStretch.spline.length = spline.length;
					jobStretch.spline.twistmode = spline.twistmode;

					if ( verts != null )
					{
						jobStretch.stretchCurve.keys = new NativeArray<Keyframe>(stretchCurve.keys, Allocator.TempJob);    // only need to set once

						jobStretch.Up				= Up;
						jobStretch.twist			= twist;
						jobStretch.ovlen			= ovlen;
						jobStretch.stretch			= stretch;
						jobStretch.usepercent		= usepercent;
						jobStretch.usetan			= usetan;
						jobStretch.normalizedInterp = path.normalizedInterp;
						jobStretch.mat				= mat;
						jobStretch.tm				= tm;
						jobStretch.invtm			= invtm;
						jobStretch.jvertices		= jverts;
						jobStretch.jsverts			= jsverts;

						jobHandle = jobStretch.Schedule(jverts.Length, mc.batchCount);
						jobHandle.Complete();

						if ( jobStretch.stretchCurve.keys.IsCreated )
							jobStretch.stretchCurve.keys.Dispose();
					}

					jobStretch.spline.points.Dispose();    // only need to set once
				}
			}
			else
			{
				if ( UseTwistCurve )
				{
					jobTwist.spline.points = new NativeArray<splinepoint>(path.splines[curve].knots.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

					MegaSpline spline = path.splines[curve];
					splinepoint sp;

					for ( int i = 0; i < spline.knots.Count; i++ )
					{
						sp.p			= spline.knots[i].p;
						sp.invec		= spline.knots[i].invec;
						sp.outvec		= spline.knots[i].outvec;
						sp.length		= spline.knots[i].length;
						sp.twist		= spline.knots[i].twist;
						sp.seglength	= spline.knots[i].seglength;

						jobTwist.spline.points[i] = sp;
					}

					jobTwist.spline.closed		= spline.closed;
					jobTwist.spline.length		= spline.length;
					jobTwist.spline.twistmode	= spline.twistmode;

					if ( verts != null )
					{
						jobTwist.twistCurve.keys = new NativeArray<Keyframe>(twistCurve.keys, Allocator.TempJob);

						jobTwist.Up					= Up;
						jobTwist.twist				= twist;
						jobTwist.ovlen				= ovlen;
						jobTwist.stretch			= stretch;
						jobTwist.usepercent			= usepercent;
						jobTwist.usetan				= usetan;
						jobTwist.normalizedInterp	= path.normalizedInterp;
						jobTwist.mat				= mat;
						jobTwist.tm					= tm;
						jobTwist.invtm				= invtm;
						jobTwist.jvertices			= jverts;
						jobTwist.jsverts			= jsverts;

						jobHandle = jobTwist.Schedule(jverts.Length, mc.batchCount);
						jobHandle.Complete();

						if ( jobTwist.twistCurve.keys.IsCreated )
							jobTwist.twistCurve.keys.Dispose();
					}

					jobTwist.spline.points.Dispose();    // only need to set once
				}
				else
				{
					job.spline.points = new NativeArray<splinepoint>(path.splines[curve].knots.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

					MegaSpline spline = path.splines[curve];
					splinepoint sp;

					for ( int i = 0; i < spline.knots.Count; i++ )
					{
						sp.p			= spline.knots[i].p;
						sp.invec		= spline.knots[i].invec;
						sp.outvec		= spline.knots[i].outvec;
						sp.length		= spline.knots[i].length;
						sp.twist		= spline.knots[i].twist;
						sp.seglength	= spline.knots[i].seglength;

						job.spline.points[i] = sp;
					}

					job.spline.closed		= spline.closed;
					job.spline.length		= spline.length;
					job.spline.twistmode	= spline.twistmode;

					if ( verts != null )
					{
						job.Up					= Up;
						job.twist				= twist;
						job.ovlen				= ovlen;
						job.stretch				= stretch;
						job.usepercent			= usepercent;
						job.usetan				= usetan;
						job.normalizedInterp	= path.normalizedInterp;
						job.mat					= mat;
						job.tm					= tm;
						job.invtm				= invtm;
						job.jvertices			= jverts;
						job.jsverts				= jsverts;

						jobHandle = job.Schedule(jverts.Length, mc.batchCount);
						jobHandle.Complete();
					}

					job.spline.points.Dispose();    // only need to set once
				}
			}
		}

		public override Vector3 Map(int i, Vector3 p)
		{
			p = tm.MultiplyPoint3x4(p);

			float alpha;
			float tws = 0.0f;

			if ( UseStretchCurve )
			{
				float str = stretchCurve.Evaluate(Mathf.Repeat(p.z * ovlen + usepercent, 1.0f)) * stretch;
				alpha = (p.z * ovlen * str) + usepercent;
			}
			else
				alpha = (p.z * ovlen * stretch) + usepercent;

			Vector3 ps	= path.InterpCurve3D(curve, alpha, path.normalizedInterp, ref tws);
			Vector3 ps1	= path.InterpCurve3D(curve, alpha + usetan, path.normalizedInterp);

			if ( path.splines[curve].closed )
				alpha = Mathf.Repeat(alpha, 1.0f);
			else
				alpha = Mathf.Clamp01(alpha);

			Quaternion	tw = Quaternion.identity;

			if ( UseTwistCurve )
			{
				float twst = twistCurve.Evaluate(alpha) * twist;
				tw = Quaternion.AngleAxis(twst + tws, Vector3.forward);
			}
			else
				tw = Quaternion.AngleAxis((twist * alpha) + tws, Vector3.forward);

			ps1.x -= ps.x;
			ps1.y -= ps.y;
			ps1.z -= ps.z;
			Quaternion rotation = Quaternion.LookRotation(ps1, Up) * tw;

			Matrix4x4 wtm = Matrix4x4.identity;
			MegaMatrix.SetTR(ref wtm, ps, rotation);
			wtm = mat * wtm;

			ps.x = (p.x * wtm[0]) + (p.y * wtm[4]) + wtm[12];
			ps.y = (p.x * wtm[1]) + (p.y * wtm[5]) + wtm[13];
			ps.z = (p.x * wtm[2]) + (p.y * wtm[6]) + wtm[14];

			return ps;
		}

		public override void ModStart(MegaModifyObject mc)
		{
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( animate )
			{
				if ( Application.isPlaying )
					percent += speed * Time.deltaTime;

				if ( usedist )
					distance = percent * 0.01f * path.splines[curve].length;
			}

			if ( path != null )
			{
				if ( usedist )
					percent = distance / path.splines[curve].length * 100.0f;

				if ( curve >= path.splines.Count )
					curve = 0;

				usepercent = percent / 100.0f;

				switch ( loopmode )
				{
					case MegaLoopMode.Clamp: usepercent = Mathf.Clamp01(usepercent); break;
					case MegaLoopMode.Loop: usepercent = Mathf.Repeat(usepercent, 1.0f); break;
					case MegaLoopMode.PingPong: usepercent = Mathf.PingPong(usepercent, 1.0f); break;
				}

				ovlen = (1.0f / path.splines[curve].length);
				usetan = (tangent * 0.01f);

				mat = Matrix4x4.identity;

				switch ( axis )
				{
					case MegaAxis.X: MegaMatrix.RotateZ(ref mat, Mathf.PI * 0.5f); break;
					case MegaAxis.Y: MegaMatrix.RotateX(ref mat, -Mathf.PI * 0.5f); break;
					case MegaAxis.Z: break;
				}

				MegaMatrix.RotateZ(ref mat, Mathf.Deg2Rad * rotate);

				SetAxis(mat);

				mat = transform.localToWorldMatrix.inverse * path.transform.localToWorldMatrix;
				return true;
			}

			return false;
		}

		public override void DrawGizmo(MegaModContext context)
		{
			SetTM();

			if ( !Prepare(context) )
				return;

			Vector3 min = context.bbox.min;
			Vector3 max = context.bbox.max;

			if ( context.mod.sourceObj != null )
				Gizmos.matrix = context.mod.sourceObj.transform.localToWorldMatrix;
			else
				Gizmos.matrix = transform.localToWorldMatrix;

			corners[0] = new Vector3(min.x, min.y, min.z);
			corners[1] = new Vector3(min.x, max.y, min.z);
			corners[2] = new Vector3(max.x, max.y, min.z);
			corners[3] = new Vector3(max.x, min.y, min.z);

			corners[4] = new Vector3(min.x, min.y, max.z);
			corners[5] = new Vector3(min.x, max.y, max.z);
			corners[6] = new Vector3(max.x, max.y, max.z);
			corners[7] = new Vector3(max.x, min.y, max.z);

			DrawEdge(corners[0], corners[1]);
			DrawEdge(corners[1], corners[2]);
			DrawEdge(corners[2], corners[3]);
			DrawEdge(corners[3], corners[0]);

			DrawEdge(corners[4], corners[5]);
			DrawEdge(corners[5], corners[6]);
			DrawEdge(corners[6], corners[7]);
			DrawEdge(corners[7], corners[4]);

			DrawEdge(corners[0], corners[4]);
			DrawEdge(corners[1], corners[5]);
			DrawEdge(corners[2], corners[6]);
			DrawEdge(corners[3], corners[7]);

			ExtraGizmo(context);
		}

		public override void GroupParams(MegaModifier root)
		{
			MegaWorldPathDeform mod = root as MegaWorldPathDeform;

			if ( mod )
			{
				percent				= mod.percent;
				stretch				= mod.stretch;
				twist				= mod.twist;
				rotate				= mod.rotate;
				axis				= mod.axis;
				flip				= mod.flip;
				path				= mod.path;
				animate				= false;
				speed				= mod.speed;
				tangent				= mod.tangent;
				UseTwistCurve		= mod.UseTwistCurve;
				twistCurve.keys		= mod.twistCurve.keys;
				UseStretchCurve		= mod.UseStretchCurve;
				stretchCurve.keys	= mod.stretchCurve.keys;
				Up					= mod.Up;
				curve				= mod.curve;
				usedist				= mod.usedist;
				distance			= mod.distance;
				loopmode			= mod.loopmode;
				gizmoPos			= mod.gizmoPos;
				gizmoRot			= mod.gizmoRot;
				gizmoScale			= mod.gizmoScale;
				bbox				= mod.bbox;
			}
		}
	}
}