using UnityEngine;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

#if false
namespace MegaFiers
{
	public class MegaMorphBase : MegaModifier
	{
		[System.Serializable]
		public class MegaMorphBlend
		{
			public float t;
			public float weight;
		}

		public List<MegaMorphChan>	chanBank = new List<MegaMorphChan>();
		public MegaMorphAnimType	animtype = MegaMorphAnimType.Bezier;
		public int					numblends;
		public List<MegaMorphBlend>	blends;

		public override void PostCopy(MegaModifier src)
		{
			MegaMorphBase mor = (MegaMorphBase)src;

			chanBank = new List<MegaMorphChan>();

			for ( int c = 0; c < mor.chanBank.Count; c++ )
			{
				MegaMorphChan chan = new MegaMorphChan();

				MegaMorphChan.Copy(mor.chanBank[c], chan);
				chanBank.Add(chan);
			}
		}

		public string[] GetChannelNames()
		{
			string[] names = new string[chanBank.Count];

			for ( int i = 0; i < chanBank.Count; i++ )
				names[i] = chanBank[i].mName;

			return names;
		}

		public MegaMorphChan GetChannel(string name)
		{
			for ( int i = 0; i < chanBank.Count; i++ )
			{
				if ( chanBank[i].mName == name )
					return chanBank[i];
			}

			return null;
		}

		public int NumChannels()
		{
			return chanBank.Count;
		}

		public void SetPercent(int i, float percent)
		{
			if ( i >= 0 && i < chanBank.Count )
				chanBank[i].Percent = percent;
		}

		public void SetPercentLim(int i, float alpha)
		{
			if ( i >= 0 && i < chanBank.Count )
			{
				if ( chanBank[i].mUseLimit )
					chanBank[i].Percent = chanBank[i].mSpinmin + ((chanBank[i].mSpinmax - chanBank[i].mSpinmin) * alpha);
				else
					chanBank[i].Percent = alpha * 100.0f;
			}
		}

		public void SetPercent(int i, float percent, float speed)
		{
			chanBank[i].SetTarget(percent, speed);
		}

		public void ResetPercent(int[] channels, float speed)
		{
			for ( int i = 0; i < channels.Length; i++ )
			{
				int chan = channels[i];
				chanBank[chan].SetTarget(0.0f, speed);
			}
		}

		public float GetPercent(int i)
		{
			if ( i >= 0 && i < chanBank.Count )
				return chanBank[i].Percent;

			return 0.0f;
		}

		public void SetAnim(float t)
		{
			if ( animtype == MegaMorphAnimType.Bezier )
			{
				for ( int i = 0; i < chanBank.Count; i++ )
				{
					if ( chanBank[i].control != null )
					{
						if ( chanBank[i].control.Times != null )
						{
							if ( chanBank[i].control.Times.Length > 0 )
								chanBank[i].Percent = chanBank[i].control.GetFloat(t);
						}
					}
				}
			}
			else
			{
				for ( int i = 0; i < chanBank.Count; i++ )
				{
					if ( chanBank[i].control != null )
					{
						if ( chanBank[i].control.Times != null )
						{
							if ( chanBank[i].control.Times.Length > 0 )
								chanBank[i].Percent = chanBank[i].control.GetHermiteFloat(t);
						}
					}
				}
			}
		}

		public void SetAnimBlend(float t, float weight)
		{
			if ( blends == null )
			{
				blends = new List<MegaMorphBlend>();

				for ( int i = 0; i < 4; i++ )
					blends.Add(new MegaMorphBlend());
			}

			blends[numblends].t = t;
			blends[numblends].weight = weight;

			numblends++;
		}

		public void ClearBlends()
		{
			numblends = 0;
		}

		public void SetChannels()
		{
			float tweight = 0.0f;
			for ( int i = 0; i < numblends; i++ )
			{
				tweight += blends[i].weight;
			}

			for ( int b = 0; b < numblends; b++ )
			{
				for ( int c = 0; c < chanBank.Count; c++ )
				{
					if ( animtype == MegaMorphAnimType.Bezier )
					{
						if ( chanBank[c].control != null )
						{
							if ( chanBank[c].control.Times != null )
							{
								if ( chanBank[c].control.Times.Length > 0 )
								{
									if ( b == 0 )
										chanBank[c].Percent = chanBank[c].control.GetFloat(blends[b].t) * (blends[b].weight / tweight);
									else
										chanBank[c].Percent += chanBank[c].control.GetFloat(blends[b].t) * (blends[b].weight / tweight);
								}
							}
						}
					}
					else
					{
						if ( chanBank[c].control != null )
						{
							if ( chanBank[c].control.Times != null )
							{
								if ( chanBank[c].control.Times.Length > 0 )
								{
									if ( b == 0 )
										chanBank[c].Percent = chanBank[c].control.GetHermiteFloat(blends[b].t) * (blends[b].weight / tweight);
									else
										chanBank[c].Percent += chanBank[c].control.GetHermiteFloat(blends[b].t) * (blends[b].weight / tweight);
								}
							}
						}
					}
				}
			}
		}
	}

	public enum MegaMorphAnimType
	{
		Bezier,
		Hermite,
	}

	[AddComponentMenu("Modifiers/Morph")]
	public class MegaMorph : MegaMorphBase
	{
		public bool					UseLimit;
		public float				Max;
		public float				Min;
		//public Vector3[]			oPoints;
		public NativeArray<Vector3>	oPoints;
		public int[]				mapping;
		public float				importScale		= 1.0f;
		public bool					flipyz			= false;
		public bool					negx			= false;
		[HideInInspector]
		public float				tolerance		= 0.0001f;
		public bool					showmapping		= false;
		public float				mappingSize		= 0.001f;
		public int					mapStart		= 0;
		public int					mapEnd			= 0;
		public NativeArray<Vector3>	dif;	// changed to public, check it doesnt break anything
		//static Vector3[]			endpoint		= new Vector3[4];
		//static Vector3[]			splinepoint		= new Vector3[4];
		//static Vector3[]			temppoint		= new Vector3[2];
		Vector3[]					p1;
		Vector3[]					p2;
		Vector3[]					p3;
		Vector3[]					p4;
		public List<float>			pers			= new List<float>(4);
		[HideInInspector]
		public int					compressedmem	= 0;
		[HideInInspector]
		public int					compressedmem1	= 0;
		[HideInInspector]
		public int					memuse			= 0;
		public bool					animate			= false;
		public float				atime			= 0.0f;
		public float				animtime		= 0.0f;
		public float				looptime		= 0.0f;
		public MegaRepeatMode		repeatMode		= MegaRepeatMode.Loop;
		public float				speed			= 1.0f;
		static int					framenum;
		// Build compressed data
		public int[]				nonMorphedVerts;
		public int[]				morphedVerts;
		public int[]				morphMappingFrom;
		public int[]				morphMappingTo;
		public int[]				nonMorphMappingFrom;
		public int[]				nonMorphMappingTo;
		// Threaded version
		Vector3[]					_verts;
		Vector3[]					_sverts;
		bool						mtmorphed;
		int[]						setStart;
		int[]						setEnd;
		int[]						copyStart;
		int[]						copyEnd;
		Job							job;
		JobHandle					jobHandle;

		public override string ModName() { return "Morph"; }
		public override string GetHelpURL() { return "?page_id=257"; }

		[BurstCompile]
		struct Job : IJobParallelFor
		{
			float3						splinepoint0;
			float3						splinepoint1;
			float3						splinepoint2;
			float3						splinepoint3;
			public int					segment;
			public int					totaltargs;
			public float				u;
			public float				curvature;
			public float				weight;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	p1;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	p2;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	p3;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	p4;
			[Unity.Collections.ReadOnly]
			public NativeArray<Vector3>	oPoints;
			public NativeArray<Vector3>	dif;

			// Job code
			public void Execute(int pointnum)
			{
				float3 vert = oPoints[pointnum];

				float length;

				float3 endpoint0 = p1[pointnum];
				float3 endpoint1 = p2[pointnum];
				float3 endpoint2 = p3[pointnum];
				float3 endpoint3 = p4[pointnum];

				if ( segment == 1 )
				{
					splinepoint0 = endpoint0;
					splinepoint3 = endpoint1;
					float3 temppoint1 = endpoint2 - endpoint0;
					float3 temppoint0 = endpoint1 - endpoint0;
					length = math.lengthsq(temppoint1);

					if ( length == 0.0f )
					{
						splinepoint1 = endpoint0;
						splinepoint2 = endpoint1;
					}
					else
					{
						splinepoint2 = endpoint1 - (math.dot(temppoint0, temppoint1) * curvature / length) * temppoint1;
						splinepoint1 = endpoint0 + curvature * (splinepoint2 - endpoint0);
					}
				}
				else
				{
					if ( segment == totaltargs )
					{
						splinepoint0 = endpoint1;
						splinepoint3 = endpoint2;
						float3 temppoint1 = endpoint2 - endpoint0;
						float3 temppoint0 = endpoint1 - endpoint2;
						length = math.lengthsq(temppoint1);

						if ( length == 0.0f )
						{
							splinepoint1 = endpoint0;
							splinepoint2 = endpoint1;
						}
						else
						{
							splinepoint1 = endpoint1 - (math.dot(temppoint1, temppoint0) * curvature / length) * temppoint1;
							splinepoint2 = endpoint2 + curvature * (splinepoint1 - endpoint2);
						}
					}
					else
					{
						float3 temppoint1 = endpoint2 - endpoint0;
						float3 temppoint0 = endpoint1 - endpoint0;
						length = math.lengthsq(temppoint1);
						splinepoint0 = endpoint1;
						splinepoint3 = endpoint2;

						if ( length == 0.0f )
							splinepoint1 = endpoint0;
						else
							splinepoint1 = endpoint1 + (math.dot(temppoint0, temppoint1) * curvature / length) * temppoint1;

						temppoint1 = endpoint3 - endpoint1;
						temppoint0 = endpoint2 - endpoint1;
						length = math.lengthsq(temppoint1);

						if ( length == 0.0f )
							splinepoint2 = endpoint1;
						else
							splinepoint2 = endpoint2 - (math.dot(temppoint0, temppoint1) * curvature / length) * temppoint1;
					}
				}

				float3 progression;

				float3 t01 = splinepoint0;

				t01.x += (splinepoint1.x - splinepoint0.x) * u;
				t01.y += (splinepoint1.y - splinepoint0.y) * u;
				t01.z += (splinepoint1.z - splinepoint0.z) * u;

				float3 t12 = splinepoint1;

				t12.x += (splinepoint2.x - splinepoint1.x) * u;
				t12.y += (splinepoint2.y - splinepoint1.y) * u;
				t12.z += (splinepoint2.z - splinepoint1.z) * u;

				float3 t02 = t01 + (t12 - t01) * u;

				t01.x = splinepoint2.x + (splinepoint3.x - splinepoint2.x) * u;
				t01.y = splinepoint2.y + (splinepoint3.y - splinepoint2.y) * u;
				t01.z = splinepoint2.z + (splinepoint3.z - splinepoint2.z) * u;

				t01.x = t12.x + (t01.x - t12.x) * u;
				t01.y = t12.y + (t01.y - t12.y) * u;
				t01.z = t12.z + (t01.z - t12.z) * u;

				progression.x = t02.x + (t01.x - t02.x) * u;
				progression.y = t02.y + (t01.y - t02.y) * u;
				progression.z = t02.z + (t01.z - t02.z) * u;

				dif[pointnum] = (progression - vert) * weight;
			}
		}





		void SetVertsJob(int j, NativeArray<Vector3> p)
		{
			switch ( j )
			{
				case 0: job.p1 = p;	break;
				case 1: job.p2 = p; break;
				case 2: job.p3 = p; break;
				case 3: job.p4 = p; break;
			}
		}


		public override void Modify(MegaModifyObject mc)
		{
#if false
			if ( nonMorphedVerts != null && nonMorphedVerts.Length > 1 )
			{
				ModifyCompressed(mc);
				return;
			}
#endif
			framenum++;
			mc.ChangeSourceVerts();

			float fChannelPercent;
			Vector3	delt;

			// cycle through channels, searching for ones to use
			bool firstchan = true;
			bool morphed = false;

			float min = 0.0f;
			float max = 100.0f;

			if ( UseLimit )
			{
				min = Min;
				max = Max;
			}

			for ( int i = 0; i < chanBank.Count; i++ )
			{
				MegaMorphChan chan = chanBank[i];
				chan.UpdatePercent();

				if ( UseLimit )
					fChannelPercent = Mathf.Clamp(chan.Percent, min, max);
				else
				{
					if ( chan.mUseLimit )
						fChannelPercent = Mathf.Clamp(chan.Percent, chan.mSpinmin, chan.mSpinmax);
					else
						fChannelPercent = Mathf.Clamp(chan.Percent, 0.0f, 100.0f);
				}

				if ( fChannelPercent != 0.0f || (fChannelPercent == 0.0f && chan.fChannelPercent != 0.0f) )
				{
					chan.fChannelPercent = fChannelPercent;

					if ( chan.mTargetCache != null && chan.mTargetCache.Count > 0 && chan.mActiveOverride )
					{
						morphed = true;

						if ( chan.mUseLimit )
						{
						}

						if ( firstchan )
						{
							firstchan = false;
							// copy these
							for ( int pointnum = 0; pointnum < oPoints.Length; pointnum++ )
								dif[pointnum] = oPoints[pointnum];
						}

						if ( chan.mTargetCache.Count == 1 )
						{
							// job this
							for ( int pointnum = 0; pointnum < oPoints.Length; pointnum++ )
							{
								delt = chan.mDeltas[pointnum];

								dif[pointnum] += delt * fChannelPercent;
								//dif[pointnum].x += delt.x * fChannelPercent;
								//dif[pointnum].y += delt.y * fChannelPercent;
								//dif[pointnum].z += delt.z * fChannelPercent;
							}
						}
						else
						{
							int totaltargs = chan.mTargetCache.Count;

							float fProgression = fChannelPercent;
							int segment = 1;
							while ( segment <= totaltargs && fProgression >= chan.GetTargetPercent(segment - 2) )
								segment++;

							if ( segment > totaltargs )
								segment = totaltargs;

							job.p4 = oPoints;

							if ( segment == 1 )
							{
								job.p1 = oPoints;
								job.p2 = chan.mTargetCache[0].points;
								job.p3 = chan.mTargetCache[1].points;
							}
							else
							{
								if ( segment == totaltargs )
								{
									int targnum = totaltargs - 1;

									for ( int j = 2; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVertsJob(j, oPoints);
										else
											SetVertsJob(j, chan.mTargetCache[targnum + 1].points);
									}
								}
								else
								{
									int targnum = segment;

									for ( int j = 3; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVertsJob(j, oPoints);
										else
											SetVertsJob(j, chan.mTargetCache[targnum + 1].points);
									}
								}
							}

							float targetpercent1 = chan.GetTargetPercent(segment - 3);
							float targetpercent2 = chan.GetTargetPercent(segment - 2);

							float top = fProgression - targetpercent1;
							float bottom = targetpercent2 - targetpercent1;
							job.u = top / bottom;

							job.segment = segment;
							job.totaltargs = totaltargs;
							job.curvature = chan.mCurvature;
							job.weight = chan.weight;
							job.oPoints = oPoints;
							job.dif = jsverts;

#if false
							// Job code
							for ( int pointnum = 0; pointnum < oPoints.Length; pointnum++ )
							{
								Vector3 vert = oPoints[pointnum];

								float length;

								Vector3 progession;

								endpoint[0] = p1[pointnum];
								endpoint[1] = p2[pointnum];
								endpoint[2] = p3[pointnum];
								endpoint[3] = p4[pointnum];

								if ( segment == 1 )
								{
									splinepoint[0] = endpoint[0];
									splinepoint[3] = endpoint[1];
									temppoint[1] = endpoint[2] - endpoint[0];
									temppoint[0] = endpoint[1] - endpoint[0];
									length = temppoint[1].sqrMagnitude;

									if ( length == 0.0f )
									{
										splinepoint[1] = endpoint[0];
										splinepoint[2] = endpoint[1];
									}
									else
									{
										splinepoint[2] = endpoint[1] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
										splinepoint[1] = endpoint[0] + chan.mCurvature * (splinepoint[2] - endpoint[0]);
									}
								}
								else
								{
									if ( segment == totaltargs )
									{
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[2];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
										{
											splinepoint[1] = endpoint[0];
											splinepoint[2] = endpoint[1];
										}
										else
										{
											splinepoint[1] = endpoint[1] - (Vector3.Dot(temppoint[1], temppoint[0]) * chan.mCurvature / length) * temppoint[1];
											splinepoint[2] = endpoint[2] + chan.mCurvature * (splinepoint[1] - endpoint[2]);
										}
									}
									else
									{
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[0];
										length = temppoint[1].sqrMagnitude;
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];

										if ( length == 0.0f )
											splinepoint[1] = endpoint[0];
										else
											splinepoint[1] = endpoint[1] + (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];

										temppoint[1] = endpoint[3] - endpoint[1];
										temppoint[0] = endpoint[2] - endpoint[1];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
											splinepoint[2] = endpoint[1];
										else
											splinepoint[2] = endpoint[2] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
									}
								}

								MegaUtils.Bez3D(out progession, ref splinepoint, u);

								dif[pointnum].x += (progession.x - vert.x) * chan.weight;
								dif[pointnum].y += (progession.y - vert.y) * chan.weight;
								dif[pointnum].z += (progession.z - vert.z) * chan.weight;
							}
#endif
						}
					}
				}
			}

			// Jobs for these as well no actually a copy will do
			if ( morphed )
			{
				for ( int i = 0; i < mapping.Length; i++ )
					jsverts[i] = dif[mapping[i]];
			}
			else
			{
				for ( int i = 0; i < verts.Length; i++ )
					jsverts[i] = jverts[i];
			}
		}



		public override bool ModLateUpdate(MegaModContext mc)
		{
			if ( animate )
			{
				if ( Application.isPlaying ) 
					animtime += Time.deltaTime * speed;

				switch ( repeatMode )
				{
					case MegaRepeatMode.Loop:	animtime = Mathf.Repeat(animtime, looptime);	break;
					case MegaRepeatMode.Clamp:	animtime = Mathf.Clamp(animtime, 0.0f, looptime); break;
				}
				SetAnim(animtime);
			}

			if ( !dif.IsCreated )	// == null )
				dif = new NativeArray<Vector3>(mc.mod.jverts.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

			return Prepare(mc);
		}

		public override bool Prepare(MegaModContext mc)
		{
			if ( chanBank != null && chanBank.Count > 0 )
				return true;

			return false;
		}

		int FindVert(Vector3 vert)
		{
			float closest = Vector3.SqrMagnitude(oPoints[0] - vert);
			int find = 0;

			for ( int i = 0; i < oPoints.Length; i++ )
			{
				float dif = Vector3.SqrMagnitude(oPoints[i] - vert);

				if ( dif < closest )
				{
					closest = dif;
					find = i;
				}
			}

			return find;
		}

		void DoMapping(Mesh mesh)
		{
			mapping = new int[mesh.vertexCount];

			for ( int v = 0; v < mesh.vertexCount; v++ )
			{
				Vector3 vert = mesh.vertices[v];
				vert.x = -vert.x;
				mapping[v] = FindVert(vert);
			}
		}

		public void DoMapping(Vector3[] verts)
		{
			mapping = new int[verts.Length];

			for ( int v = 0; v < verts.Length; v++ )
			{
				mapping[v] = FindVert(verts[v]);
			}
		}

		void SetVerts(int j, Vector3[] p)
		{
			switch ( j )
			{
				case 0: p1 = p;	break;
				case 1: p2 = p; break;
				case 2: p3 = p; break;
				case 3: p4 = p; break;
			}
		}

		void SetVerts(MegaMorphChan chan, int j, Vector3[] p)
		{
			switch ( j )
			{
				case 0: chan.p1 = p; break;
				case 1: chan.p2 = p; break;
				case 2: chan.p3 = p; break;
				case 3: chan.p4 = p; break;
			}
		}

#if false
		public override void Modify(MegaModifyObject mc)
		{
			if ( nonMorphedVerts != null && nonMorphedVerts.Length > 1 )
			{
				ModifyCompressed(mc);
				return;
			}

			framenum++;
			mc.ChangeSourceVerts();

			float fChannelPercent;
			Vector3	delt;

			// cycle through channels, searching for ones to use
			bool firstchan = true;
			bool morphed = false;

			float min = 0.0f;
			float max = 100.0f;

			if ( UseLimit )
			{
				min = Min;
				max = Max;
			}

			for ( int i = 0; i < chanBank.Count; i++ )
			{
				MegaMorphChan chan = chanBank[i];
				chan.UpdatePercent();

				if ( UseLimit )
					fChannelPercent = Mathf.Clamp(chan.Percent, min, max);
				else
				{
					if ( chan.mUseLimit )
						fChannelPercent = Mathf.Clamp(chan.Percent, chan.mSpinmin, chan.mSpinmax);
					else
						fChannelPercent = Mathf.Clamp(chan.Percent, 0.0f, 100.0f);
				}

				if ( fChannelPercent != 0.0f || (fChannelPercent == 0.0f && chan.fChannelPercent != 0.0f) )
				{
					chan.fChannelPercent = fChannelPercent;

					if ( chan.mTargetCache != null && chan.mTargetCache.Count > 0 && chan.mActiveOverride )
					{
						morphed = true;

						if ( chan.mUseLimit )
						{
						}

						if ( firstchan )
						{
							firstchan = false;
							for ( int pointnum = 0; pointnum < oPoints.Length; pointnum++ )
								dif[pointnum] = oPoints[pointnum];
						}

						if ( chan.mTargetCache.Count == 1 )
						{
							for ( int pointnum = 0; pointnum < oPoints.Length; pointnum++ )
							{
								delt = chan.mDeltas[pointnum];

								dif[pointnum].x += delt.x * fChannelPercent;
								dif[pointnum].y += delt.y * fChannelPercent;
								dif[pointnum].z += delt.z * fChannelPercent;
							}
						}
						else
						{
							int totaltargs = chan.mTargetCache.Count;

							float fProgression = fChannelPercent;
							int segment = 1;
							while ( segment <= totaltargs && fProgression >= chan.GetTargetPercent(segment - 2) )
								segment++;

							if ( segment > totaltargs )
								segment = totaltargs;

							p4 = oPoints;

							if ( segment == 1 )
							{
								p1 = oPoints;
								p2 = chan.mTargetCache[0].points;
								p3 = chan.mTargetCache[1].points;
							}
							else
							{
								if ( segment == totaltargs )
								{
									int targnum = totaltargs - 1;

									for ( int j = 2; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVerts(j, oPoints);
										else
											SetVerts(j, chan.mTargetCache[targnum + 1].points);
									}
								}
								else
								{
									int targnum = segment;

									for ( int j = 3; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVerts(j, oPoints);
										else
											SetVerts(j, chan.mTargetCache[targnum + 1].points);
									}
								}
							}

							float targetpercent1 = chan.GetTargetPercent(segment - 3);
							float targetpercent2 = chan.GetTargetPercent(segment - 2);

							float top = fProgression - targetpercent1;
							float bottom = targetpercent2 - targetpercent1;
							float u = top / bottom;

							// Job code
							for ( int pointnum = 0; pointnum < oPoints.Length; pointnum++ )
							{
								Vector3 vert = oPoints[pointnum];

								float length;

								Vector3 progession;

								endpoint[0] = p1[pointnum];
								endpoint[1] = p2[pointnum];
								endpoint[2] = p3[pointnum];
								endpoint[3] = p4[pointnum];

								if ( segment == 1 )
								{
									splinepoint[0] = endpoint[0];
									splinepoint[3] = endpoint[1];
									temppoint[1] = endpoint[2] - endpoint[0];
									temppoint[0] = endpoint[1] - endpoint[0];
									length = temppoint[1].sqrMagnitude;

									if ( length == 0.0f )
									{
										splinepoint[1] = endpoint[0];
										splinepoint[2] = endpoint[1];
									}
									else
									{
										splinepoint[2] = endpoint[1] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
										splinepoint[1] = endpoint[0] + chan.mCurvature * (splinepoint[2] - endpoint[0]);
									}
								}
								else
								{
									if ( segment == totaltargs )
									{
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[2];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
										{
											splinepoint[1] = endpoint[0];
											splinepoint[2] = endpoint[1];
										}
										else
										{
											splinepoint[1] = endpoint[1] - (Vector3.Dot(temppoint[1], temppoint[0]) * chan.mCurvature / length) * temppoint[1];
											splinepoint[2] = endpoint[2] + chan.mCurvature * (splinepoint[1] - endpoint[2]);
										}
									}
									else
									{
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[0];
										length = temppoint[1].sqrMagnitude;
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];

										if ( length == 0.0f )
											splinepoint[1] = endpoint[0];
										else
											splinepoint[1] = endpoint[1] + (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];

										temppoint[1] = endpoint[3] - endpoint[1];
										temppoint[0] = endpoint[2] - endpoint[1];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
											splinepoint[2] = endpoint[1];
										else
											splinepoint[2] = endpoint[2] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
									}
								}

								MegaUtils.Bez3D(out progession, ref splinepoint, u);

								dif[pointnum].x += (progession.x - vert.x) * chan.weight;
								dif[pointnum].y += (progession.y - vert.y) * chan.weight;
								dif[pointnum].z += (progession.z - vert.z) * chan.weight;
							}
						}
					}
				}
			}

			if ( morphed )
			{
				for ( int i = 0; i < mapping.Length; i++ )
					sverts[i] = dif[mapping[i]];
			}
			else
			{
				for ( int i = 0; i < verts.Length; i++ )
					sverts[i] = verts[i];
			}
		}
#endif
		bool Changed(int v, int c)
		{
			for ( int t = 0; t < chanBank[c].mTargetCache.Count; t++ )
			{
				if ( !oPoints[v].Equals(chanBank[c].mTargetCache[t].points[v]) )
					return true;
			}

			return false;
		}

		public void Compress()
		{
			if ( oPoints != null )
			{
				List<int>	altered = new List<int>();

				int count = 0;

				for ( int c = 0; c < chanBank.Count; c++ )
				{
					altered.Clear();

					for ( int v = 0; v < oPoints.Length; v++ )
					{
						if ( Changed(v, c) )
							altered.Add(v);
					}

					count += altered.Count;
				}

				Debug.Log("Compressed will only morph " + count + " points instead of " + (oPoints.Length * chanBank.Count));
				compressedmem = count * 12;
			}
		}

#if false
		public void ModifyCompressed(MegaModifyObject mc)
		{
			framenum++;
			mc.ChangeSourceVerts();

			float fChannelPercent;
			Vector3	delt;

			// cycle through channels, searching for ones to use
			bool firstchan = true;
			bool morphed = false;

			for ( int i = 0; i < chanBank.Count; i++ )
			{
				MegaMorphChan chan = chanBank[i];
				chan.UpdatePercent();

				if ( chan.mUseLimit )
					fChannelPercent = Mathf.Clamp(chan.Percent, chan.mSpinmin, chan.mSpinmax);
				else
					fChannelPercent = Mathf.Clamp(chan.Percent, 0.0f, 100.0f);

				if ( fChannelPercent != 0.0f || (fChannelPercent == 0.0f && chan.fChannelPercent != 0.0f) )
				{
					chan.fChannelPercent = fChannelPercent;

					if ( chan.mTargetCache != null && chan.mTargetCache.Count > 0 && chan.mActiveOverride )
					{
						morphed = true;

						if ( chan.mUseLimit )
						{
						}

						if ( firstchan )
						{
							firstchan = false;
							// Save a int array of morphedpoints and use that, then only dealing with changed info
							for ( int pointnum = 0; pointnum < morphedVerts.Length; pointnum++ )
							{
								// this will change when we remove points
								int p = morphedVerts[pointnum];
								dif[p] = oPoints[p];
							}
						}

						if ( chan.mTargetCache.Count == 1 )
						{
							for ( int pointnum = 0; pointnum < morphedVerts.Length; pointnum++ )
							{
								int p = morphedVerts[pointnum];
								delt = chan.mDeltas[p];

								dif[p].x += delt.x * fChannelPercent;
								dif[p].y += delt.y * fChannelPercent;
								dif[p].z += delt.z * fChannelPercent;
							}
						}
						else
						{
							int totaltargs = chan.mTargetCache.Count;

							float fProgression = fChannelPercent;
							int segment = 1;
							while ( segment <= totaltargs && fProgression >= chan.GetTargetPercent(segment - 2) )
								segment++;

							if ( segment > totaltargs )
								segment = totaltargs;

							p4 = oPoints;

							if ( segment == 1 )
							{
								p1 = oPoints;
								p2 = chan.mTargetCache[0].points;
								p3 = chan.mTargetCache[1].points;
							}
							else
							{
								if ( segment == totaltargs )
								{
									int targnum = totaltargs - 1;

									for ( int j = 2; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVerts(j, oPoints);
										else
											SetVerts(j, chan.mTargetCache[targnum + 1].points);
									}
								}
								else
								{
									int targnum = segment;

									for ( int j = 3; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVerts(j, oPoints);
										else
											SetVerts(j, chan.mTargetCache[targnum + 1].points);
									}
								}
							}

							float targetpercent1 = chan.GetTargetPercent(segment - 3);
							float targetpercent2 = chan.GetTargetPercent(segment - 2);

							float top = fProgression - targetpercent1;
							float bottom = targetpercent2 - targetpercent1;
							float u = top / bottom;

							for ( int pointnum = 0; pointnum < morphedVerts.Length; pointnum++ )
							{
								int p = morphedVerts[pointnum];
								Vector3 vert = oPoints[p];

								float length;

								Vector3 progession;

								endpoint[0] = p1[p];
								endpoint[1] = p2[p];
								endpoint[2] = p3[p];
								endpoint[3] = p4[p];

								if ( segment == 1 )
								{
									splinepoint[0] = endpoint[0];
									splinepoint[3] = endpoint[1];
									temppoint[1] = endpoint[2] - endpoint[0];
									temppoint[0] = endpoint[1] - endpoint[0];
									length = temppoint[1].sqrMagnitude;

									if ( length == 0.0f )
									{
										splinepoint[1] = endpoint[0];
										splinepoint[2] = endpoint[1];
									}
									else
									{
										splinepoint[2] = endpoint[1] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
										splinepoint[1] = endpoint[0] + chan.mCurvature * (splinepoint[2] - endpoint[0]);
									}
								}
								else
								{
									if ( segment == totaltargs )
									{
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[2];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
										{
											splinepoint[1] = endpoint[0];
											splinepoint[2] = endpoint[1];
										}
										else
										{
											splinepoint[1] = endpoint[1] - (Vector3.Dot(temppoint[1], temppoint[0]) * chan.mCurvature / length) * temppoint[1];
											splinepoint[2] = endpoint[2] + chan.mCurvature * (splinepoint[1] - endpoint[2]);
										}
									}
									else
									{
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[0];
										length = temppoint[1].sqrMagnitude;
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];

										if ( length == 0.0f )
											splinepoint[1] = endpoint[0];
										else
											splinepoint[1] = endpoint[1] + (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];

										temppoint[1] = endpoint[3] - endpoint[1];
										temppoint[0] = endpoint[2] - endpoint[1];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
											splinepoint[2] = endpoint[1];
										else
											splinepoint[2] = endpoint[2] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
									}
								}

								MegaUtils.Bez3D(out progession, ref splinepoint, u);

								dif[p].x += progession.x - vert.x;
								dif[p].y += progession.y - vert.y;
								dif[p].z += progession.z - vert.z;
							}
						}
					}
				}
			}

			if ( morphed )
			{
				for ( int i = 0; i < morphMappingFrom.Length; i++ )
					sverts[morphMappingTo[i]] = dif[morphMappingFrom[i]];

				for ( int i = 0; i < nonMorphMappingFrom.Length; i++ )
					sverts[nonMorphMappingTo[i]] = oPoints[nonMorphMappingFrom[i]];
			}
			else
			{
				for ( int i = 0; i < verts.Length; i++ )
					sverts[i] = verts[i];
			}
		}
#endif

#if true
		[ContextMenu("Compress Morphs")]
		public void BuildCompress()
		{
			bool[]	altered = new bool[oPoints.Length];

			int count = 0;

			for ( int c = 0; c < chanBank.Count; c++ )
			{
				for ( int v = 0; v < oPoints.Length; v++ )
				{
					if ( Changed(v, c) )
						altered[v] = true;
				}
			}

			for ( int i = 0; i < altered.Length; i++ )
			{
				if ( altered[i] )
					count++;
			}

			morphedVerts = new int[count];
			nonMorphedVerts = new int[oPoints.Length - count];

			int mindex = 0;
			int nmindex = 0;

			List<int>	mappedFrom = new List<int>();
			List<int>	mappedTo = new List<int>();

			for ( int i = 0; i < oPoints.Length; i++ )
			{
				if ( altered[i] )
				{
					morphedVerts[mindex++] = i;

					for ( int m = 0; m < mapping.Length; m++ )
					{
						if ( mapping[m] == i )
						{
							mappedFrom.Add(i);
							mappedTo.Add(m);
						}
					}
				}
				else
					nonMorphedVerts[nmindex++] = i;
			}

			morphMappingFrom = mappedFrom.ToArray();
			morphMappingTo = mappedTo.ToArray();

			mappedFrom.Clear();
			mappedTo.Clear();

			for ( int i = 0; i < oPoints.Length; i++ )
			{
				if ( !altered[i] )
				{
					for ( int m = 0; m < mapping.Length; m++ )
					{
						if ( mapping[m] == i )
						{
							mappedFrom.Add(i);
							mappedTo.Add(m);
						}
					}
				}
			}

			nonMorphMappingFrom = mappedFrom.ToArray();
			nonMorphMappingTo = mappedTo.ToArray();

			compressedmem = morphedVerts.Length * chanBank.Count * 12;
		}

#if false
		public override void PrepareMT(MegaModifyObject mc, int cores)
		{
			PrepareForMT(mc, cores);
		}

		public override void DoWork(MegaModifyObject mc, int index, int start, int end, int cores)
		{
			ModifyCompressedMT(mc, index, cores);
		}
#endif
#endif
#if false
		public void PrepareForMT(MegaModifyObject mc, int cores)
		{
			if ( setStart == null )
				BuildMorphVertInfo(cores);

			// cycle through channels, searching for ones to use
			mtmorphed = false;

			for ( int i = 0; i < chanBank.Count; i++ )
			{
				MegaMorphChan chan = chanBank[i];
				chan.UpdatePercent();

				float fChannelPercent;

				if ( chan.mUseLimit )
					fChannelPercent = Mathf.Clamp(chan.Percent, chan.mSpinmin, chan.mSpinmax);
				else
					fChannelPercent = Mathf.Clamp(chan.Percent, 0.0f, 100.0f);

				if ( fChannelPercent != 0.0f || (fChannelPercent == 0.0f && chan.fChannelPercent != 0.0f) )
				{
					chan.fChannelPercent = fChannelPercent;

					if ( chan.mTargetCache != null && chan.mTargetCache.Count > 0 && chan.mActiveOverride )
					{
						mtmorphed = true;

						if ( chan.mTargetCache.Count > 1 )
						{
							int totaltargs = chan.mTargetCache.Count;

							chan.fProgression = chan.fChannelPercent;
							chan.segment = 1;
							while ( chan.segment <= totaltargs && chan.fProgression >= chan.GetTargetPercent(chan.segment - 2) )
								chan.segment++;

							if ( chan.segment > totaltargs )
								chan.segment = totaltargs;

							chan.p4 = oPoints;

							if ( chan.segment == 1 )
							{
								chan.p1 = oPoints;
								chan.p2 = chan.mTargetCache[0].points;
								chan.p3 = chan.mTargetCache[1].points;
							}
							else
							{
								if ( chan.segment == totaltargs )
								{
									int targnum = totaltargs - 1;

									for ( int j = 2; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVerts(chan, j, oPoints);
										else
											SetVerts(chan, j, chan.mTargetCache[targnum + 1].points);
									}
								}
								else
								{
									int targnum = chan.segment;

									for ( int j = 3; j >= 0; j-- )
									{
										targnum--;
										if ( targnum == -2 )
											SetVerts(chan, j, oPoints);
										else
											SetVerts(chan, j, chan.mTargetCache[targnum + 1].points);
									}
								}
							}
						}
					}
				}
			}

			if ( !mtmorphed )
			{
				for ( int i = 0; i < verts.Length; i++ )
					sverts[i] = verts[i];
			}
			else
			{
				for ( int pointnum = 0; pointnum < morphedVerts.Length; pointnum++ )
				{
					int p = morphedVerts[pointnum];
					dif[p] = oPoints[p];
				}
			}
		}

		public void ModifyCompressedMT(MegaModifyObject mc, int tindex, int cores)
		{
			if ( !mtmorphed )
				return;

			int step = morphedVerts.Length / cores;
			int startvert = (tindex * step);
			int endvert = startvert + step;

			if ( tindex == cores - 1 )
				endvert = morphedVerts.Length;

			framenum++;
			Vector3	delt;

			// cycle through channels, searching for ones to use
			Vector3[]	endpoint	= new Vector3[4];	// These in channel class
			Vector3[]	splinepoint	= new Vector3[4];
			Vector3[]	temppoint	= new Vector3[2];

			for ( int i = 0; i < chanBank.Count; i++ )
			{
				MegaMorphChan chan = chanBank[i];

				if ( chan.fChannelPercent != 0.0f )
				{
					if ( chan.mTargetCache != null && chan.mTargetCache.Count > 0 && chan.mActiveOverride )
					{
						if ( chan.mTargetCache.Count == 1 )
						{
							for ( int pointnum = startvert; pointnum < endvert; pointnum++ )
							{
								int p = morphedVerts[pointnum];
								delt = chan.mDeltas[p];

								dif[p].x += delt.x * chan.fChannelPercent;
								dif[p].y += delt.y * chan.fChannelPercent;
								dif[p].z += delt.z * chan.fChannelPercent;
							}
						}
						else
						{
							float targetpercent1 = chan.GetTargetPercent(chan.segment - 3);
							float targetpercent2 = chan.GetTargetPercent(chan.segment - 2);

							float top = chan.fProgression - targetpercent1;
							float bottom = targetpercent2 - targetpercent1;
							float u = top / bottom;

							for ( int pointnum = startvert; pointnum < endvert; pointnum++ )
							{
								int p = morphedVerts[pointnum];
								Vector3 vert = oPoints[p];

								float length;

								Vector3 progession;

								endpoint[0] = chan.p1[p];
								endpoint[1] = chan.p2[p];
								endpoint[2] = chan.p3[p];
								endpoint[3] = chan.p4[p];

								if ( chan.segment == 1 )
								{
									splinepoint[0] = endpoint[0];
									splinepoint[3] = endpoint[1];
									temppoint[1] = endpoint[2] - endpoint[0];
									temppoint[0] = endpoint[1] - endpoint[0];
									length = temppoint[1].sqrMagnitude;

									if ( length == 0.0f )
									{
										splinepoint[1] = endpoint[0];
										splinepoint[2] = endpoint[1];
									}
									else
									{
										splinepoint[2] = endpoint[1] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
										splinepoint[1] = endpoint[0] + chan.mCurvature * (splinepoint[2] - endpoint[0]);
									}
								}
								else
								{
									if ( chan.segment == chan.mTargetCache.Count )
									{
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[2];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
										{
											splinepoint[1] = endpoint[0];
											splinepoint[2] = endpoint[1];
										}
										else
										{
											splinepoint[1] = endpoint[1] - (Vector3.Dot(temppoint[1], temppoint[0]) * chan.mCurvature / length) * temppoint[1];
											splinepoint[2] = endpoint[2] + chan.mCurvature * (splinepoint[1] - endpoint[2]);
										}
									}
									else
									{
										temppoint[1] = endpoint[2] - endpoint[0];
										temppoint[0] = endpoint[1] - endpoint[0];
										length = temppoint[1].sqrMagnitude;
										splinepoint[0] = endpoint[1];
										splinepoint[3] = endpoint[2];

										if ( length == 0.0f )
											splinepoint[1] = endpoint[0];
										else
											splinepoint[1] = endpoint[1] + (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];

										temppoint[1] = endpoint[3] - endpoint[1];
										temppoint[0] = endpoint[2] - endpoint[1];
										length = temppoint[1].sqrMagnitude;

										if ( length == 0.0f )
											splinepoint[2] = endpoint[1];
										else
											splinepoint[2] = endpoint[2] - (Vector3.Dot(temppoint[0], temppoint[1]) * chan.mCurvature / length) * temppoint[1];
									}
								}

								MegaUtils.Bez3D(out progession, ref splinepoint, u);

								dif[p].x += progession.x - vert.x;
								dif[p].y += progession.y - vert.y;
								dif[p].z += progession.z - vert.z;
							}
						}
					}
				}
			}
		}

		public override void DoneMT(MegaModifyObject mod)
		{
			if ( mtmorphed )
			{
				for ( int i = 0; i < morphMappingFrom.Length; i++ )
					sverts[morphMappingTo[i]] = dif[morphMappingFrom[i]];

				for ( int i = 0; i < nonMorphMappingFrom.Length; i++ )
					sverts[nonMorphMappingTo[i]] = oPoints[nonMorphMappingFrom[i]];
			}
		}
#endif
		int Find(int index)
		{
			int f = morphedVerts[index];

			for ( int i = 0; i < morphMappingFrom.Length; i++ )
			{
				if ( morphMappingFrom[i] > f )
					return i;
			}

			return morphMappingFrom.Length - 1;
		}

		void BuildMorphVertInfo(int cores)
		{
			int step = morphedVerts.Length / cores;

			setStart	= new int[cores];
			setEnd		= new int[cores];
			copyStart	= new int[cores];
			copyEnd		= new int[cores];

			int start = 0;
			int fv = 0;

			for ( int i = 0; i < cores; i++ )
			{
				setStart[i] = start;
				if ( i < cores - 1 )
				{
					setEnd[i] = Find(fv + step);
				}
				start = setEnd[i];
				fv += step;
			}

			setEnd[cores - 1] = morphMappingFrom.Length;

			// copys can be simple split as nothing happens to them
			start = 0;
			step = nonMorphMappingFrom.Length / cores;

			for ( int i = 0; i < cores; i++ )
			{
				copyStart[i] = start;
				copyEnd[i] = start + step;
				start += step;
			}

			copyEnd[cores - 1] = nonMorphMappingFrom.Length;
		}

		public void SetAnimTime(float t)
		{
			animtime = t;

			switch ( repeatMode )
			{
				case MegaRepeatMode.Loop:	animtime = Mathf.Repeat(animtime, looptime);	break;
				case MegaRepeatMode.Clamp:	animtime = Mathf.Clamp(animtime, 0.0f, looptime); break;
			}
			SetAnim(animtime);
		}
	}
}
#endif