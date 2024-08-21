using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace MegaFiers
{
	public enum MegaInterpMethod
	{
		None,
		Linear,
		Bez,
	}

	public enum MegaBlendAnimMode
	{
		Replace,
		Additive,
	}

	[AddComponentMenu("Modifiers/Point Cache")]
	public class MegaPointCache : MegaModifier
	{
		[Adjust()]
		public float					time			= 0.0f;
		[Adjust()]
		public bool						animated		= false;
		[Adjust()]
		public float					speed			= 1.0f;
		[Adjust()]
		public float					maxtime			= 1.0f;
		[Adjust()]
		public MegaRepeatMode			LoopMode		= MegaRepeatMode.PingPong;
		[Adjust()]
		public MegaInterpMethod			interpMethod	= MegaInterpMethod.Linear;
		[Adjust()]
		public float					weight			= 1.0f;
		public bool						framedelay		= true;
		public MegaBlendAnimMode		blendMode		= MegaBlendAnimMode.Additive;	// local space
		float							t;
		float							alpha			= 0.0f;
		float							dalpha			= 0.0f;
		int								sindex;
		int								sindex1;
		int								jsindex;
		int								jsindex1;
		public bool						showmapping		= false;
		public float					mappingSize		= 0.001f;
		public int						mapStart		= 0;
		public int						mapEnd			= 0;
		public bool						havemapping		= false;
		public float					scl				= 1.0f;
		public bool						flipyz			= false;
		public bool						negx			= false;
		public bool						negz			= false;
		public float					adjustscl		= 1.0f;
		public Vector3					adjustoff		= Vector3.zero;
		bool							skipframe		= true;
		JobLinearAbs					joblinearabs;
		JobLinearRel					joblinearrel;
		JobHandle						jobHandle;
		public MegaPointCacheFile		cacheFile;
		public int						everyNth		= 1;
		public Vector3					mapRot			= Vector3.zero;
		public Vector3					mapPos			= Vector3.zero;
		public Vector3					mapScl			= Vector3.one;
		public Vector3					importRot		= Vector3.zero;
		public Vector3					importPos		= Vector3.zero;
		public Vector3					importScl		= Vector3.one;
		float							lasttime;
		float							lastweight;
		MegaRepeatMode					lastloop;
		MegaInterpMethod				lastinterp;
		MegaBlendAnimMode				lastblend;

		public override string ModName()	{ return "Point Cache"; }
		public override string GetHelpURL() { return "?page_id=1335"; }

		[BurstCompile]
		struct JobLinearAbs : IJobParallelFor
		{
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>		points;
			public int						sindex;
			public int						sindex1;
			public float					dalpha;
			public NativeArray<Vector3>		jsverts;

			public void Execute(int vi)
			{
				jsverts[vi] = math.lerp(points[sindex + vi], points[sindex1 + vi], dalpha);
			}
		}

		[BurstCompile]
		struct JobLinearRel : IJobParallelFor
		{
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>		points;
			public int						sindex;
			public int						sindex1;
			public float					dalpha;
			public float					weight;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>		jvertices;
			public NativeArray<Vector3>		jsverts;

			public void Execute(int vi)
			{
				Vector3 p = points[sindex + vi];
				Vector3 p1 = points[sindex1 + vi];

				p += ((p1 - p) * dalpha) - jvertices[vi];
				jsverts[vi] = jvertices[vi] + (p * weight);
			}
		}

		public override bool Changed()
		{
			if ( t != lasttime )
			{
				lasttime = t;
				return true;
			}

			if ( weight != lastweight )
			{
				lastweight = weight;
				return true;
			}

			if ( LoopMode != lastloop )
			{
				lastloop = LoopMode;
				return true;
			}

			if ( interpMethod != lastinterp )
			{
				lastinterp = interpMethod;
				return true;
			}

			if ( blendMode != lastblend )
			{
				lastblend = blendMode;
				return true;
			}

			return false;
		}

		public override void Modify(MegaModifyObject mc)
		{
			//ModifyNoJobs(mc);
			//return;
			if ( cacheFile && cacheFile.Verts != null )
			{
				BuildArrays(mc);

				switch ( interpMethod )
				{
					case MegaInterpMethod.Linear:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive:
								joblinearrel.sindex		= jsindex;
								joblinearrel.sindex1	= jsindex1;
								joblinearrel.points		= cacheFile.points;
								joblinearrel.dalpha		= dalpha;
								joblinearrel.jvertices	= jverts;
								joblinearrel.jsverts	= jsverts;
								joblinearrel.weight		= weight;

								jobHandle = joblinearrel.Schedule(mc.jverts.Length, mc.batchCount);
								jobHandle.Complete();
								break;

							case MegaBlendAnimMode.Replace:
								joblinearabs.sindex		= jsindex;
								joblinearabs.sindex1	= jsindex1;
								joblinearabs.points		= cacheFile.points;
								joblinearabs.dalpha		= dalpha;
								joblinearabs.jsverts	= jsverts;

								jobHandle = joblinearabs.Schedule(mc.jverts.Length, mc.batchCount);
								jobHandle.Complete();
								break;
						}
						break;

					case MegaInterpMethod.Bez:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive:
								joblinearrel.sindex		= jsindex;
								joblinearrel.sindex1	= jsindex1;
								joblinearrel.points		= cacheFile.points;
								joblinearrel.dalpha		= dalpha;
								joblinearrel.jvertices	= jverts;
								joblinearrel.jsverts	= jsverts;
								joblinearrel.weight		= weight;

								jobHandle = joblinearrel.Schedule(mc.jverts.Length, mc.batchCount);
								jobHandle.Complete();
								break;

							case MegaBlendAnimMode.Replace:
								joblinearabs.sindex		= jsindex;
								joblinearabs.sindex1	= jsindex1;
								joblinearabs.points		= cacheFile.points;
								joblinearabs.dalpha		= dalpha;
								joblinearabs.jsverts	= jsverts;

								jobHandle = joblinearabs.Schedule(mc.jverts.Length, mc.batchCount);
								jobHandle.Complete();
								break;
						}
						break;

					case MegaInterpMethod.None:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive:
								joblinearrel.sindex		= jsindex;
								joblinearrel.sindex1	= jsindex1;
								joblinearrel.points		= cacheFile.points;
								joblinearrel.dalpha		= dalpha;
								joblinearrel.jvertices	= jverts;
								joblinearrel.jsverts	= jsverts;
								joblinearrel.weight		= weight;

								jobHandle = joblinearrel.Schedule(mc.jverts.Length, mc.batchCount);
								jobHandle.Complete();
								break;

							case MegaBlendAnimMode.Replace:
								joblinearabs.sindex		= jsindex;
								joblinearabs.sindex1	= jsindex1;
								joblinearabs.points		= cacheFile.points;
								joblinearabs.dalpha		= dalpha;
								joblinearabs.jsverts	= jsverts;

								jobHandle = joblinearabs.Schedule(mc.jverts.Length, mc.batchCount);
								jobHandle.Complete();
								break;
						}
						break;
				}
			}
			else
			{
				// job for this or copy?
				for ( int i = 0; i < verts.Length; i++ )
					sverts[i] = verts[i];
			}
		}

		public override void ModifyNoJobs(MegaModifyObject mc)
		{
			if ( cacheFile && cacheFile.Verts != null )
			{
				BuildArrays(mc);

				switch ( interpMethod )
				{
					case MegaInterpMethod.Linear:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive: LinearRel(mc, 0, mc.jverts.Length); break;
							case MegaBlendAnimMode.Replace: LinearAbs(mc, 0, mc.jverts.Length); break;
						}
						break;

					case MegaInterpMethod.Bez:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive: LinearRel(mc, 0, mc.jverts.Length); break;
							case MegaBlendAnimMode.Replace: LinearAbs(mc, 0, mc.jverts.Length); break;
						}
						break;

					case MegaInterpMethod.None:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive: NoInterpRel(mc, 0, mc.jverts.Length); break;
							case MegaBlendAnimMode.Replace: NoInterpAbs(mc, 0, mc.jverts.Length); break;
						}
						break;
				}
			}
			else
			{
				for ( int i = 0; i < verts.Length; i++ )
					sverts[i] = verts[i];
			}
		}

		void LinearAbs(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3>	points = cacheFile.points;

			for ( int i = start; i < end; i++ )
				jsverts[i] = math.lerp(points[jsindex + i], points[jsindex1 + i], dalpha);
		}

		void LinearAbsWeighted(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3> points = cacheFile.points;

			for ( int i = start; i < end; i++ )
			{
				Vector3 p = points[i + jsindex];
				Vector3 p1 = points[i + jsindex1];
				p.x = p.x + ((p1.x - p.x) * dalpha);
				p.y = p.y + ((p1.y - p.y) * dalpha);
				p.z = p.z + ((p1.z - p.z) * dalpha);

				float w = 1.0f;	//mc.selection[cacheFile.Verts[i].indices[0]];
				p1 = jverts[i];

				p = p1 + ((p - p1) * w);
				jsverts[i] = p;
			}
		}

		void LinearRel(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3> points = cacheFile.points;

			for ( int i = start; i < end; i++ )
			{
				Vector3 basep = jverts[i];

				Vector3 p = points[i + jsindex];
				Vector3 p1 = points[i + jsindex1];

				p.x += (((p1.x - p.x) * dalpha) - basep.x);
				p.y += (((p1.y - p.y) * dalpha) - basep.y);
				p.z += (((p1.z - p.z) * dalpha) - basep.z);

				p1 = jverts[i];

				p.x = p1.x + (p.x * weight);
				p.y = p1.y + (p.y * weight);
				p.z = p1.z + (p.z * weight);

				jsverts[i] = p;
			}
		}

		void LinearRelWeighted(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3> points = cacheFile.points;

			for ( int i = start; i < end; i++ )
			{
				//int ix = cacheFile.Verts[i].indices[0];

				Vector3 basep = jverts[i];

				Vector3 p = points[i + jsindex];
				Vector3 p1 = points[i + jsindex1];
				p.x += (((p1.x - p.x) * dalpha) - basep.x);
				p.y += (((p1.y - p.y) * dalpha) - basep.y);
				p.z += (((p1.z - p.z) * dalpha) - basep.z);

				float w = 1.0f;	//mc.selection[cacheFile.Verts[i].indices[0]] * weight;

				p1 = jverts[i];

				p.x = p1.x + (p.x * w);
				p.y = p1.y + (p.y * w);
				p.z = p1.z + (p.z * w);

				jsverts[i] = p;
			}
		}

		void NoInterpAbs(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3> points = cacheFile.points;

			for ( int i = start; i < end; i++ )
				jsverts[i] = points[i + jsindex];
		}

		void NoInterpAbsWeighted(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3> points = cacheFile.points;

			for ( int i = start; i < end; i++ )
			{
				Vector3 p = points[i + jsindex];

				float w = 1.0f;	//mc.selection[cacheFile.Verts[i].indices[0]] * weight;

				Vector3 p1 = jverts[i];

				p = p1 + ((p - p1) * w);

				jsverts[i] = p;
			}
		}

		void NoInterpRel(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3> points = cacheFile.points;

			for ( int i = start; i < end; i++ )
			{
				Vector3 p = points[i + jsindex] - jverts[i];

				Vector3 p1 = jverts[i];

				p.x = p1.x + (p.x * weight);
				p.y = p1.y + (p.y * weight);
				p.z = p1.z + (p.z * weight);

				jsverts[i] = p;
			}
		}

		void NoInterpRelWeighted(MegaModifyObject mc, int start, int end)
		{
			NativeArray<Vector3> points = cacheFile.points;

			for ( int i = start; i < end; i++ )
			{
				int ix = cacheFile.Verts[i].indices[0];
				Vector3 p = points[i + jsindex] - jverts[i];

				float w = 1.0f;	//mc.selection[cacheFile.Verts[i].indices[0]] * weight;

				Vector3 p1 = jverts[i];

				p = p1 + ((p - p1) * w);

				jsverts[i] = p;
			}
		}

		public void ModifyInstance(MegaModifyObject mc, float itime)
		{
			if ( cacheFile && cacheFile.Verts != null )
			{
				switch ( LoopMode )
				{
					case MegaRepeatMode.Loop:		t = Mathf.Repeat(itime, maxtime); break;
					case MegaRepeatMode.PingPong:	t = Mathf.PingPong(itime, maxtime); break;
					case MegaRepeatMode.Clamp:		t = Mathf.Clamp(itime, 0.0f, maxtime); break;
				}

				alpha = t / maxtime;

				float val = (float)(cacheFile.Verts[0].points.Length - 1) * alpha;

				sindex = (int)val;
				dalpha = val - sindex;
				if ( sindex == cacheFile.Verts[0].points.Length - 1 )
				{
					sindex1 = sindex;
					dalpha = 0.0f;
				}
				else
					sindex1 = sindex + 1;

				jsindex = sindex * mc.jverts.Length;
				jsindex1 = sindex1 * mc.jverts.Length;

				switch ( interpMethod )
				{
					case MegaInterpMethod.Linear:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive: LinearRel(mc, 0, cacheFile.Verts.Length); break;
							case MegaBlendAnimMode.Replace: LinearAbs(mc, 0, cacheFile.Verts.Length); break;
						}
						break;

					case MegaInterpMethod.Bez:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive: LinearRel(mc, 0, cacheFile.Verts.Length); break;
							case MegaBlendAnimMode.Replace: LinearAbs(mc, 0, cacheFile.Verts.Length); break;
						}
						break;

					case MegaInterpMethod.None:
						switch ( blendMode )
						{
							case MegaBlendAnimMode.Additive: NoInterpRel(mc, 0, cacheFile.Verts.Length); break;
							case MegaBlendAnimMode.Replace: NoInterpAbs(mc, 0, cacheFile.Verts.Length); break;
						}
						break;
				}
			}
			else
			{
				for ( int i = 0; i < verts.Length; i++ )
					sverts[i] = verts[i];
			}
		}

		public void SetAnim(float _t)
		{
			time = _t;
			t = _t;
			skipframe = true;
		}

		public override bool ModLateUpdate(MegaModContext mc)
		{
			return prepared;
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( cacheFile == null || cacheFile.Verts == null || cacheFile.Verts.Length == 0 )
				return false;

			if ( animated )
			{
				if ( framedelay && skipframe )
					skipframe = false;
				else
				{
					if ( Application.isPlaying )
						time += Time.deltaTime * speed;
				}
			}

			switch ( LoopMode )
			{
				case MegaRepeatMode.Loop: t = Mathf.Repeat(time, maxtime); break;
				case MegaRepeatMode.PingPong: t = Mathf.PingPong(time, maxtime); break;
				case MegaRepeatMode.Clamp: t = Mathf.Clamp(time, 0.0f, maxtime); break;
			}

			alpha = t / maxtime;

			float val = (float)(cacheFile.Verts[0].points.Length - 1) * alpha;

			sindex = (int)val;
			dalpha = val - sindex;

			if ( sindex == cacheFile.Verts[0].points.Length - 1 )
			{
				sindex1 = sindex;
				dalpha = 0.0f;
			}
			else
				sindex1 = sindex + 1;

			jsindex = sindex * mc.mod.jverts.Length;
			jsindex1 = sindex1 * mc.mod.jverts.Length;

			if ( cacheFile && cacheFile.Verts != null && cacheFile.Verts.Length > 0 && cacheFile.Verts[0].indices != null && cacheFile.Verts[0].indices.Length > 0 )
				return true;

			return false;
		}

		public void BuildArrays(MegaModifyObject mod)
		{
			if ( !cacheFile.points.IsCreated )
				cacheFile.points = new NativeArray<Vector3>(cacheFile.cacheValues, Allocator.Persistent);
		}
	}
}