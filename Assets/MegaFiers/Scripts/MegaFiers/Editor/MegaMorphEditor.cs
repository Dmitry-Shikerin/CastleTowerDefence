using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using Unity.Collections;

#if false
namespace MegaFiers
{
	public class MegaTargetMesh
	{
		public string			name;
		public List<Vector3>	verts = new List<Vector3>();
		public List<int>		faces = new List<int>();

		static public List<MegaTargetMesh> LoadTargets(string path, float scale, bool flipzy, bool negx)
		{
			List<MegaTargetMesh>	targets = new List<MegaTargetMesh>();

			MegaTargetMesh current = null;

			StreamReader stream = File.OpenText(path);
			string entireText = stream.ReadToEnd();
			stream.Close();

			entireText = entireText.Replace("\n", "\r\n");

			List<Vector3>	verts = new List<Vector3>();

			using ( StringReader reader = new StringReader(entireText) )
			{
				string currentText = reader.ReadLine();

				char[] splitIdentifier = { ' ' };
				string[] brokenString;
				string name = "";

				Vector3 p = Vector3.zero;

				while ( currentText != null )
				{
					if ( !currentText.StartsWith("v ") && !currentText.StartsWith("g ") && !currentText.StartsWith("f ") )
					{
						currentText = reader.ReadLine();
						if ( currentText != null )
							currentText = currentText.Replace("  ", " ");
					}
					else
					{
						currentText = currentText.Trim();
						brokenString = currentText.Split(splitIdentifier, 50);
						switch ( brokenString[0] )
						{
							case "f":
								if ( verts.Count > 0 )
								{
									current = new MegaTargetMesh();
									current.name = name;
									current.verts = new List<Vector3>(verts);
									current.faces = new List<int>();
									targets.Add(current);

									verts.Clear();
								}
								break;

							case "g":
								name = brokenString[1];
								break;

							case "v":
								p.x = System.Convert.ToSingle(brokenString[1]) * scale;
								if ( negx )
								{
									p.x = -p.x;
								}

								if ( flipzy )
								{
									p.y = System.Convert.ToSingle(brokenString[3]) * scale;
									p.z = System.Convert.ToSingle(brokenString[2]) * scale;
								}
								else
								{
									p.y = System.Convert.ToSingle(brokenString[2]) * scale;
									p.z = System.Convert.ToSingle(brokenString[3]) * scale;
								}
								verts.Add(p);
								break;
						}

						currentText = reader.ReadLine();
						if ( currentText != null )
							currentText = currentText.Replace("  ", " ");
					}
				}
			}

			return targets;
		}
	}

	public class MegaMorphEditor : Editor
	{
		Stack<Color>			bcol			= new Stack<Color>();
		Stack<Color>			ccol			= new Stack<Color>();
		Stack<Color>			col				= new Stack<Color>();
		bool					extraparams		= false;
		private MegaModifier	src;
		bool					showmodparams	= false;
		bool					showimport		= false;
		bool					showchannels	= true;
		bool					showadvanced	= false;
		MegaMorphChan			currentChan;
		MegaMorphTarget			currentTarget;
		MegaModifyObject		mo				= null;
		static string			lastpath		= " ";
		static public Color		ChanCol1		= new Color(0.44f, 0.67f, 1.0f);
		static public Color		ChanCol2		= new Color(1.0f, 0.67f, 0.44f);

		private void OnEnable()
		{
			src = target as MegaModifier;
		}

		int FindVert(Vector3 vert, List<Vector3> verts, float tolerance)
		{
			float closest = Vector3.SqrMagnitude(verts[0] - vert);
			int find = 0;

			for ( int i = 0; i < verts.Count; i++ )
			{
				float dif = Vector3.SqrMagnitude(verts[i] - vert);

				if ( dif < closest )
				{
					closest = dif;
					find = i;
				}
			}

			if ( closest > tolerance )
				return -1;

			return find;
		}

		int FindVert(Vector3 vert, List<Vector3> verts, float tolerance, float scl, bool flipyz, bool negx, int vn)
		{
			int find = 0;

			if ( negx )
				vert.x = -vert.x;

			if ( flipyz )
			{
				float z = vert.z;
				vert.z = vert.y;
				vert.y = z;
			}

			vert /= scl;

			float closest = Vector3.SqrMagnitude(verts[0] - vert);

			for ( int i = 0; i < verts.Count; i++ )
			{
				float dif = Vector3.SqrMagnitude(verts[i] - vert);

				if ( dif < closest )
				{
					closest = dif;
					find = i;
				}
			}

			if ( closest > tolerance )
				return -1;

			return find;
		}

		int FindVert(Vector3 vert, List<Vector3> verts, float tolerance, float scl, bool flipyz, bool negx, bool negy, int vn)
		{
			int find = 0;

			if ( negx )
				vert.x = -vert.x;

			if ( flipyz )
			{
				float z = vert.z;
				vert.z = vert.y;
				vert.y = z;
			}

			if ( negy )
				vert.y = -vert.y;

			vert /= scl;

			float closest = Vector3.SqrMagnitude(verts[0] - vert);

			for ( int i = 0; i < verts.Count; i++ )
			{
				float dif = Vector3.SqrMagnitude(verts[i] - vert);

				if ( dif < closest )
				{
					closest = dif;
					find = i;
				}
			}

			if ( closest > tolerance )
				return -1;

			return find;
		}

		bool FindMatch(Vector3 vert, Vector3[] verts, float tolerance)
		{
			float closest = Vector3.SqrMagnitude(verts[0] - vert);

			for ( int i = 0; i < verts.Length; i++ )
			{
				float dif = Vector3.SqrMagnitude(verts[i] - vert);

				if ( dif < closest )
					closest = dif;
			}

			if ( closest > tolerance )
				return false;

			return true;
		}

		bool DoMapping(MegaModifyObject mod, MegaMorph morph, MegaTargetMesh tm, int[] mapping, float scale, bool flipyz, bool negx)
		{
			int step = mod.jverts.Length / 10;
			int count = 0;
			for ( int i = 0; i < mod.jverts.Length; i++ )
			{
				count--;
				if ( count < 0 )
				{
					count = step;

					float a = (float)i / (float)mod.jverts.Length;

					EditorUtility.DisplayProgressBar("Mapping", "Mapping vertex " + i, a);
				}
				mapping[i] = FindVert(mod.jverts[i], tm.verts, morph.tolerance, scale, flipyz, negx, i);

				if ( mapping[i] == -1 )
				{
					EditorUtility.ClearProgressBar();
					return false;
				}
			}

			EditorUtility.ClearProgressBar();
			return true;
		}

		bool DoMapping(MegaModifyObject mod, MegaMorph morph, MegaTargetMesh tm, int[] mapping, float scale, bool flipyz, bool negx, bool negy)
		{
			int step = mod.jverts.Length / 10;
			int count = 0;
			for ( int i = 0; i < mod.jverts.Length; i++ )
			{
				count--;
				if ( count < 0 )
				{
					count = step;
					float a = (float)i / (float)mod.jverts.Length;
					EditorUtility.DisplayProgressBar("Mapping", "Mapping vertex " + i, a);
				}
				mapping[i] = FindVert(mod.jverts[i], tm.verts, morph.tolerance, scale, flipyz, negx, negy, i);

				if ( mapping[i] == -1 )
				{
					EditorUtility.ClearProgressBar();
					return false;
				}
			}

			EditorUtility.ClearProgressBar();
			return true;
		}

		bool TryMapping(List<MegaTargetMesh> targets, MegaMorph morph)
		{
			MegaModifyObject mod = morph.GetComponent<MegaModifyObject>();

			if ( mod == null )
			{
				EditorUtility.DisplayDialog("Missing ModifyObject!", "No ModifyObject script found on the object", "OK");
				return false;
			}

			int[] mapping = new int[mod.jverts.Length];

			for ( int t = 0; t < targets.Count;	t++ )
			{
				MegaTargetMesh tm = targets[t];

				Vector3 min1,max1;
				Vector3 min2,max2;

				Vector3 ex1 = MegaUtils.Extents(mod.jverts, out min1, out max1);
				Vector3 ex2 = MegaUtils.Extents(tm.verts, out min2, out max2);

				float d1 = ex1.x;
				float d2 = ex2.x;

				float scl = d1 / d2;
				bool flipyz = false;
				bool negx = false;

				bool mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);
			
				if ( !mapped )
				{
					flipyz = true;
					mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);
					if ( !mapped )
					{
						flipyz = false;
						negx = true;
						mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);
						if ( !mapped )
						{
							flipyz = true;
							mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);
						}
					}
				}

				if ( mapped )
				{
					morph.importScale	= scl;
					morph.flipyz		= flipyz;
					morph.negx			= negx;
					morph.mapping		= mapping;
					morph.oPoints		= new NativeArray<Vector3>(tm.verts.ToArray(), Allocator.Persistent);

					for ( int i = 0; i < morph.oPoints.Length; i++ )
					{
						Vector3 p = morph.oPoints[i];

						if ( negx )
							p.x = -p.x;

						if ( flipyz )
						{
							float z = p.z;
							p.z = p.y;
							p.y = z;
						}

						morph.oPoints[i] = p * morph.importScale;
					}

					return true;
				}
			}

			return false;
		}

		void LoadBase(MegaMorph morph)
		{
			string filename = EditorUtility.OpenFilePanel("Morph Base", lastpath, "obj");

			if ( filename == null || filename.Length < 1 )
				return;

			lastpath = filename;
			List<MegaTargetMesh> targets = MegaTargetMesh.LoadTargets(filename, 1.0f, false, false);

			if ( targets != null && targets.Count > 0 )
			{
				if ( !TryMapping(targets, morph) )
				{
					EditorUtility.DisplayDialog("Mapping Failed!", "Mapping of " + System.IO.Path.GetFileNameWithoutExtension(filename) + " failed!", "OK");
					EditorUtility.ClearProgressBar();
					return;
				}
			}
		}

		void LoadTargets(MegaMorphChan channel)
		{
			MegaMorph mr = (MegaMorph)target;

			string filename = EditorUtility.OpenFilePanel("Morph Targets", lastpath, "obj");
			if ( filename == null || filename.Length < 1 )
				return;

			lastpath = filename;
			List<MegaTargetMesh> targets = MegaTargetMesh.LoadTargets(filename, mr.importScale, mr.flipyz, mr.negx);

			if ( targets != null )
			{
				if ( channel.mName == "Empty" )
					channel.mName = System.IO.Path.GetFileNameWithoutExtension(filename);

				for ( int i = 0; i < targets.Count; i++ )
				{
					MegaTargetMesh tm = targets[i];

					if ( tm.verts.Count != mr.oPoints.Length )
						EditorUtility.DisplayDialog("Target Vertex count mismatch!", "Target " + tm.name + " has wrong number of verts", "OK");
					else
					{
						MegaMorphTarget mt = channel.GetTarget(tm.name);

						if ( mt == null )
						{
							mt = new MegaMorphTarget();
							mt.name = tm.name;
							channel.mTargetCache.Add(mt);
						}

						mt.points = new NativeArray<Vector3>(tm.verts.ToArray(), Allocator.Persistent);
					}
				}

				channel.ResetPercent();
				channel.Rebuild(mr);
			}

			mr.BuildCompress();
		}

		void LoadTarget(MegaMorphTarget mt)
		{
			MegaMorph mr = (MegaMorph)target;
			string filename = EditorUtility.OpenFilePanel("Morph Target", lastpath, "obj");
			if ( filename == null || filename.Length < 1 )
				return;

			lastpath = filename;
			List<MegaTargetMesh> targets = MegaTargetMesh.LoadTargets(filename, mr.importScale, mr.flipyz, mr.negx);

			if ( targets != null && targets.Count > 0 )
			{
				MegaTargetMesh tm = targets[0];

				if ( tm.verts.Count != mr.oPoints.Length )
					EditorUtility.DisplayDialog("Target Vertex count mismatch!", "Target " + tm.name + " has wrong number of verts", "OK");
				else
				{
					mt.points = new NativeArray<Vector3>(tm.verts.ToArray(), Allocator.Persistent);
					mt.name = tm.name;
				}
			}
		}

		void SwapTargets(MegaMorphChan chan, int t1, int t2)
		{
			if ( t1 >= 0 && t1 < chan.mTargetCache.Count && t2 >= 0 && t2 < chan.mTargetCache.Count && t1 != t2 )
			{
				MegaMorphTarget mt1 = chan.mTargetCache[t1];
				MegaMorphTarget mt2 = chan.mTargetCache[t2];
				float per = mt1.percent;
				mt1.percent = mt2.percent;
				mt2.percent = per;
				chan.mTargetCache.RemoveAt(t1);
				chan.mTargetCache.Insert(t2, mt1);
				EditorUtility.SetDirty(target);
			}
		}

		void DisplayTarget(MegaMorph morph, MegaMorphChan channel, MegaMorphTarget mt, int num)
		{
			PushCols();
			EditorGUI.indentLevel = 1;
			MegaEditorGUILayout.Text(target, "Name", ref mt.name);
			MegaEditorGUILayout.Slider(target, "Percent", ref mt.percent, 0.0f, 100.0f);

			EditorGUILayout.BeginHorizontal();

			if ( mt.points == null || mt.points.Length != morph.oPoints.Length)
				GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
			else
				GUI.backgroundColor = new Color(0.0f, 1.0f, 0.0f);

			if ( GUILayout.Button("Load") )
			{
				Undo.RecordObject(target, "Load");
				LoadTarget(mt);
			}

			GUI.backgroundColor = new Color(1.0f, 0.5f, 0.5f);
			if ( GUILayout.Button("Delete") )
			{
				Undo.RecordObject(target, "Delete");
				MegaMorphTarget mt0 = channel.mTargetCache[0];

				channel.mTargetCache.Remove(mt);
				channel.ResetPercent();

				if ( channel.mTargetCache.Count > 0 && channel.mTargetCache[0] != mt0 )
					channel.Rebuild(morph);
			}

			GUI.backgroundColor = new Color(1.0f, 1.0f, 0.5f);
			if ( GUILayout.Button("Up") )
			{
				if ( num > 0 )
				{
					Undo.RecordObject(target, "Up");
					SwapTargets(channel, num, num - 1);

					if ( num == 1 )
						channel.Rebuild(morph);
				}
			}

			GUI.backgroundColor = new Color(0.5f, 1.0f, 1.0f);
			if ( GUILayout.Button("Dn") )
			{
				if ( num < channel.mTargetCache.Count - 1 )
				{
					Undo.RecordObject(target, "Down");
					SwapTargets(channel, num, num + 1);

					if ( num == 0 )
						channel.Rebuild(morph);
				}
			}

			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel = 0;
			PopCols();
		}

		void PushCols()
		{
			bcol.Push(GUI.backgroundColor);
			ccol.Push(GUI.contentColor);
			col.Push(GUI.color);
		}

		void PopCols()
		{
			GUI.backgroundColor = bcol.Pop();
			GUI.contentColor = ccol.Pop();
			GUI.color = col.Pop();
		}

		void DisplayChannel(MegaMorph morph, MegaMorphChan channel, int num)
		{
			if ( GUILayout.Button(num + " - " + channel.mName) )
				channel.showparams = !channel.showparams;

			float min = 0.0f;
			float max = 100.0f;
			if ( morph.UseLimit )
			{
				min = morph.Min;
				max = morph.Max;
			}

			GUI.backgroundColor = new Color(1, 1, 1);
			if ( channel.showparams )
			{
				MegaEditorGUILayout.Text(target, "Name", ref channel.mName);

				if ( channel.mTargetCache != null && channel.mTargetCache.Count > 0 )
				{
					MegaEditorGUILayout.Toggle(target, "Active", ref channel.mActiveOverride);
				
					if ( morph.UseLimit )
						MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, min, max);
					else
					{
						if ( channel.mUseLimit )
							MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
						else
							MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, 0.0f, 100.0f);
					}
					MegaEditorGUILayout.Float(target, "Tension", ref channel.mCurvature);
					MegaEditorGUILayout.Slider(target, "Weight", ref channel.weight, 0.0f, 1.0f);
				}

				MegaEditorGUILayout.Toggle(target, "Use Limit", ref channel.mUseLimit);

				if ( channel.mUseLimit )
				{
					MegaEditorGUILayout.Float(target, "Min", ref channel.mSpinmin);
					MegaEditorGUILayout.Float(target, "Max", ref channel.mSpinmax);
				}

				EditorGUILayout.BeginHorizontal();
				PushCols();
				GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
				if ( GUILayout.Button("Load Targets") )
				{
					Undo.RecordObject(target, "Load Targets");
					if ( channel.mTargetCache == null )
						channel.mTargetCache = new List<MegaMorphTarget>();

					LoadTargets(channel);
				}

				GUI.backgroundColor = new Color(0.5f, 1.0f, 0.5f);
				if ( GUILayout.Button("Add Target") )
				{
					Undo.RecordObject(target, "Add Target");
					if ( channel.mTargetCache == null )
						channel.mTargetCache = new List<MegaMorphTarget>();

					MegaMorphTarget mt = new MegaMorphTarget();
					channel.mTargetCache.Add(mt);
					channel.ResetPercent();
				}

				GUI.backgroundColor = new Color(1.5f, 0.5f, 0.5f);
				if ( GUILayout.Button("Delete Channel") )
				{
					Undo.RecordObject(target, "Delete Channel");
					morph.chanBank.Remove(channel);
				}

				EditorGUILayout.EndHorizontal();

				PopCols();

				if ( channel.mTargetCache != null && channel.mTargetCache.Count > 0 )
				{
					channel.showtargets = EditorGUILayout.Foldout(channel.showtargets, "Targets");

					if ( channel.showtargets )
					{
						if ( channel.mTargetCache != null )
						{
							for ( int i = 0; i < channel.mTargetCache.Count; i++ )
								DisplayTarget(morph, channel, channel.mTargetCache[i], i);
						}
					}
				}
			}
			else
			{
				if ( channel.mActiveOverride && channel.mTargetCache != null && channel.mTargetCache.Count > 0 )
				{
					if ( morph.UseLimit )
						MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, min, max);
					else
					{
						if ( channel.mUseLimit )
							MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
						else
							MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, 0.0f, 100.0f);
					}
				}
			}
		}

		void DisplayChannelLim(MegaMorph morph, MegaMorphChan channel, int num)
		{
			float min = 0.0f;
			float max = 100.0f;
			if ( morph.UseLimit )
			{
				min = morph.Min;
				max = morph.Max;
			}

			GUI.backgroundColor = new Color(1, 1, 1);
			if ( channel.mActiveOverride && channel.mTargetCache != null && channel.mTargetCache.Count > 0 )
			{
				if ( morph.UseLimit )
					MegaEditorGUILayout.Slider(target, channel.mName, ref channel.Percent, min, max);
				else
				{
					if ( channel.mUseLimit )
						MegaEditorGUILayout.Slider(target, channel.mName, ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
					else
						MegaEditorGUILayout.Slider(target, channel.mName, ref channel.Percent, 0.0f, 100.0f);
				}
			}
		}

		void ImportParams(MegaMorph morph)
		{
			showimport = EditorGUILayout.Foldout(showimport, "Import Params");

			if ( showimport )
			{
				MegaEditorGUILayout.Float(target, "Import Scale", ref morph.importScale);
				MegaEditorGUILayout.Toggle(target, "FlipYZ", ref morph.flipyz);
				MegaEditorGUILayout.Toggle(target, "Negate X", ref morph.negx);
				MegaEditorGUILayout.Float(target, "Tolerance", ref morph.tolerance);
			}
		}

		public override void OnInspectorGUI()
		{
			MegaMorph morph = (MegaMorph)target;

			PushCols();

			if ( GUILayout.Button("Import Morph File") )
			{
				Undo.RecordObject(target, "Import");
				LoadMorph();
				EditorUtility.SetDirty(target);
			}

			showmodparams = EditorGUILayout.Foldout(showmodparams, "Modifier Common Params");

			if ( showmodparams )
			{
				MegaEditorGUILayout.Text(target, "Label", ref morph.Label);
				MegaEditorGUILayout.Int(target, "MaxLOD", ref morph.MaxLOD);
				MegaEditorGUILayout.Toggle(target, "Mod Enabled", ref morph.ModEnabled);
				MegaEditorGUILayout.Toggle(target, "Display Gizmo", ref morph.DisplayGizmo);
				//MegaEditorGUILayout.Int(target, "Order", ref morph.Order);
				MegaEditorGUILayout.Color(target, "Giz Col 1", ref morph.gizCol1);
				MegaEditorGUILayout.Color(target, "Giz Col 2", ref morph.gizCol2);
			}

			showadvanced = EditorGUILayout.Foldout(showadvanced, "Advanced Params");

			if ( showadvanced )
			{
				if ( morph.oPoints != null )
				{
					MegaEditorGUILayout.BeginToggle(target, "Show Mapping", ref morph.showmapping);
					MegaEditorGUILayout.Int(target, "StartVert", ref morph.mapStart, 0, morph.oPoints.Length);
					MegaEditorGUILayout.Int(target, "endVert", ref morph.mapEnd, 0, morph.oPoints.Length);
					MegaEditorGUILayout.Slider(target, "Size", ref morph.mappingSize, 0.0005f, 0.1f);
					MegaEditorGUILayout.EndToggle();
				}

				MegaEditorGUILayout.Slider(target, "Tolerance", ref morph.tolerance, 0.0f, 0.01f);
			}

			MegaEditorGUILayout.BeginToggle(target, "Use Limits", ref morph.UseLimit);
			MegaEditorGUILayout.Float(target, "Min", ref morph.Min);
			MegaEditorGUILayout.Float(target, "Max", ref morph.Max);
			MegaEditorGUILayout.EndToggle();

			MegaEditorGUILayout.Toggle(target, "Animate", ref morph.animate);

			if ( morph.animate )
			{
				MegaEditorGUILayout.Float(target, "AnimTime", ref morph.animtime);
				MegaEditorGUILayout.Float(target, "LoopTime", ref morph.looptime);
				MegaEditorGUILayout.Float(target, "Speed", ref morph.speed);
				MegaEditorGUILayout.RepeatMode(target, "RepeatMode", ref morph.repeatMode);
			}

			EditorGUILayout.BeginHorizontal();
			PushCols();
			if ( morph.mapping == null || morph.mapping.Length == 0 )
				GUI.backgroundColor = Color.red;
			else
				GUI.backgroundColor = Color.green;

			if ( GUILayout.Button("Load Mapping") )
				LoadBase(morph);

			PopCols();

			if ( GUILayout.Button("Add Channel") )
			{
				Undo.RecordObject(target, "Add Channel");
				if ( morph.chanBank == null )
					morph.chanBank = new List<MegaMorphChan>();

				MegaMorphChan nc = new MegaMorphChan();
				nc.mName = "Empty";
				morph.chanBank.Add(nc);
			}

			EditorGUILayout.EndHorizontal();

			string bname = "Hide Channels";

			if ( !showchannels )
				bname = "Show Channels";

			if ( GUILayout.Button(bname) )
				showchannels = !showchannels;

			MegaEditorGUILayout.Toggle(target, "Compact Display", ref morph.limitchandisplay);

			if ( showchannels && morph.chanBank != null )
			{
				if ( morph.limitchandisplay )
				{
					MegaEditorGUILayout.Int(target, "Start", ref morph.startchannel);
					MegaEditorGUILayout.Int(target, "Display", ref morph.displaychans);
					if ( morph.displaychans < 0 )
						morph.displaychans = 0;

					if ( morph.startchannel >= morph.chanBank.Count - 1 )
						morph.startchannel = morph.chanBank.Count - 1;

					if ( morph.startchannel < 0 )
						morph.startchannel = 0;

					int end = morph.startchannel + morph.displaychans;
					if ( end >= morph.chanBank.Count )
						end = morph.chanBank.Count;

					for ( int i = morph.startchannel; i < end; i++ )
					{
						PushCols();

						if ( (i & 1) == 0 )
							GUI.backgroundColor = ChanCol1;
						else
							GUI.backgroundColor = ChanCol2;

						DisplayChannelLim(morph, morph.chanBank[i], i);
						PopCols();
					}
				}
				else
				{
					for ( int i = 0; i < morph.chanBank.Count; i++ )
					{
						PushCols();

						if ( (i & 1) == 0 )
							GUI.backgroundColor = ChanCol1;
						else
							GUI.backgroundColor = ChanCol2;

						DisplayChannel(morph, morph.chanBank[i], i);
						PopCols();
					}
				}
			}

			extraparams = EditorGUILayout.Foldout(extraparams, "Extra Params");

			if ( extraparams )
			{
				MegaEditorGUILayout.Color(target, "Channel Col 1", ref ChanCol1);
				MegaEditorGUILayout.Color(target, "Channel Col 2", ref ChanCol2);

				if ( morph.compressedmem == 0 )
				{
					morph.memuse = CalcMemoryUsage(morph);
					morph.Compress();
				}
				EditorGUILayout.LabelField("Memory: ", (morph.memuse / 1024) + "KB");
				EditorGUILayout.LabelField("Channel Compressed: ", (morph.compressedmem / 1024) + "KB");
			}

			PopCols();

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}

		int MorphedVerts(MegaMorph mr, MegaMorphChan channel)
		{
			int count = 0;

			for ( int v = 0; v < mr.oPoints.Length; v++ )
			{
				Vector3 p = mr.oPoints[v];

				bool morphed = false;

				for ( int i = 0; i < channel.mTargetCache.Count; i++ )
				{
					MegaMorphTarget mt = channel.mTargetCache[i];

					if ( !p.Equals(mt.points[v]) )
					{
						morphed = true;
						break;
					}
				}

				if ( morphed )
					count++;
			}

			return count;
		}

		void ChannelMapping(MegaMorph mr, MegaMorphChan mc)
		{
			mc.mapping = new int[mr.oPoints.Length];

			for ( int i = 0; i < mr.oPoints.Length; i++ )
			{
				mc.mapping[i] = i;
			}
		}

		void CompressChannel(MegaMorph mr, MegaMorphChan mc)
		{
			mc.mapping = new int[mr.oPoints.Length];

			for ( int i = 0; i < mr.oPoints.Length; i++ )
				mc.mapping[i] = i;
		}

		int CalcCompressedMemory(MegaMorph mr)
		{
			int mem = 0;

			for ( int i = 0; i < mr.chanBank.Count; i++ )
			{
				MegaMorphChan mc = mr.chanBank[i];

				int mv = MorphedVerts(mr, mc);
				int m = mv * 12 * mc.mTargetCache.Count;
				mem += m;
				EditorGUILayout.LabelField(mc.mName, "Verts: " + mv + " mem: " + m);
			}

			EditorGUILayout.LabelField("Total: ", (mem / 1024) + "KB");

			return mem;
		}

		int CalcMemoryUsage(MegaMorph mr)
		{
			int mem = 0;

			for ( int i = 0; i < mr.chanBank.Count; i++ )
			{
				MegaMorphChan mc = mr.chanBank[i];
				mem += mc.mTargetCache.Count * 12 * mr.oPoints.Length;
			}

			return mem;
		}

		public void ParseFile(String assetpath, ParseClassCallbackType cb)
		{
			FileStream fs = new FileStream(assetpath, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);

			BinaryReader br = new BinaryReader(fs);

			bool processing = true;

			while ( processing )
			{
				string classname = MegaParse.ReadString(br);

				if ( classname == "Done" )
					break;

				int	chunkoff = br.ReadInt32();
				long fpos = fs.Position;

				cb(classname, br);

				fs.Position = fpos + chunkoff;
			}

			br.Close();
		}

		void MorphCallback(string classname, BinaryReader br)
		{
			switch ( classname )
			{
				case "Morph":	LoadMorph(br);	break;
			}
		}

		int startchan = 0;
		void LoadMorph()
		{
			MegaMorph mr = (MegaMorph)target;

			string filename = EditorUtility.OpenFilePanel("Morph File", lastpath, "mor");

			if ( filename == null || filename.Length < 1 )
				return;

			lastpath = filename;

			startchan = 0;
			bool opt = true;
			if ( mr.chanBank != null && mr.chanBank.Count > 0 )
				opt = EditorUtility.DisplayDialog("Channel Import Option", "Channels already present, do you want to 'Add' or 'Replace' channels in this file?", "Add", "Replace");

			if ( mr.chanBank != null )
			{
				if ( opt )
					startchan = mr.chanBank.Count;
				else
					mr.chanBank.Clear();
			}

			ParseFile(filename, MorphCallback);

			mr.animate = false;
			float looptime = 0.0f;
			for ( int i = 0; i < mr.chanBank.Count; i++ )
			{
				MegaMorphChan mc = mr.chanBank[i];

				if ( mc.control != null )
				{
					mr.animate = true;
					if ( mc.control.Times != null && mc.control.Times.Length > 0 )
					{
						float t = mc.control.Times[mc.control.Times.Length - 1];
						if ( t > looptime )
							looptime = t;
					}
				}
			}

			if ( mr.animate )
				mr.looptime = looptime;
			mr.compressedmem = 0;
			mr.BuildCompress();
		}

		public void LoadMorph(BinaryReader br)
		{
			MegaParse.Parse(br, ParseMorph);
		}

		bool AnimCallback(BinaryReader br, string id)
		{
			MegaMorph mr = (MegaMorph)target;

			switch ( id )
			{
				case "Chan":
					int cn = br.ReadInt32() + startchan;
					if ( cn < mr.chanBank.Count )
						currentChan = mr.chanBank[cn];
					else
					{
						Debug.LogWarning("Morph File has animation data for a missing target, check your original file and delete unused channels");
						currentChan = null;
					}
					break;

				case "Anim":
					MegaBezFloatKeyControl con = LoadAnim(br);
					if ( currentChan != null )
					{
						currentChan.control = con;
						mr.animtype = MegaMorphAnimType.Bezier;
					}
					break;

				case "MayaAnim":
					MegaBezFloatKeyControl hcon = LoadMayaAnim(br);
					if ( currentChan != null )
					{
						currentChan.control = hcon;
						mr.animtype = MegaMorphAnimType.Hermite;
					}
					break;

				default: return false;
			}

			return true;
		}

		void LoadAnimation(MegaMorph mr, BinaryReader br)
		{
			MegaParse.Parse(br, AnimCallback);
		}

		public bool ParseMorph(BinaryReader br, string id)
		{
			MegaMorph mr = (MegaMorph)target;

			switch ( id )
			{
				case "Max": mr.Max = br.ReadSingle(); break;
				case "Min": mr.Min = br.ReadSingle(); break;
				case "UseLim": mr.UseLimit = (br.ReadInt32() == 1); break;

				case "StartPoints":
					MegaTargetMesh tm = new MegaTargetMesh();
					tm.verts = MegaParse.ReadP3l(br);
					if ( !TryMapping1(tm, mr) )
					{
						EditorUtility.DisplayDialog("Mapping Failed!", "Mapping failed! Please check the Morph page on the MegaFiers website for reasons for this", "OK");
						EditorUtility.ClearProgressBar();
						return false;
					}
					break;

				case "Channel":
					MegaMorphChan chan = LoadChan(br);
					if ( chan != null )
						mr.chanBank.Add(chan);
					break;

				case "Animation":
					LoadAnimation(mr, br);
					break;
				default:	return false;
			}

			return true;
		}

		public MegaMorphChan LoadChan(BinaryReader br)
		{
			MegaMorphChan chan = new MegaMorphChan();

			chan.control = null;
			chan.showparams = false;
			chan.mTargetCache = new List<MegaMorphTarget>();
			currentChan = chan;

			MegaParse.Parse(br, ParseChan);

			for ( int i = 0; i < chan.mTargetCache.Count; i++ )
			{
				if ( chan.mTargetCache[i].points == null || chan.mTargetCache[i].points.Length == 0 )
					return null;
			}

			MegaMorph mr = (MegaMorph)target;
			chan.Rebuild(mr);
			return chan;
		}

		public static MegaBezFloatKeyControl LoadAnim(BinaryReader br)
		{
			return MegaParseBezFloatControl.LoadBezFloatKeyControl(br);
		}

		public static MegaBezFloatKeyControl LoadMayaAnim(BinaryReader br)
		{
			return MegaParseBezFloatControl.LoadBezFloatKeyControl(br);
		}

		public bool ParseChan(BinaryReader br, string id)
		{
			switch ( id )
			{
				case "Target":		currentChan.mTargetCache.Add(LoadTarget(br)); break;
				case "Name":		currentChan.mName = MegaParse.ReadString(br); break;
				case "Percent":		currentChan.Percent = br.ReadSingle(); break;
				case "SpinMax":		currentChan.mSpinmax = br.ReadSingle(); break;
				case "SpinMin":		currentChan.mSpinmin = br.ReadSingle(); break;
				case "UseLim":		currentChan.mUseLimit = (br.ReadInt32() == 1); break;
				case "Override":	currentChan.mActiveOverride = (br.ReadInt32() == 1); break;
				case "Curve":		currentChan.mCurvature = br.ReadSingle(); break;
			}

			return true;
		}

		void ConvertPoints(Vector3[] verts)
		{
			MegaMorph mr = (MegaMorph)target;

			for ( int i = 0; i < verts.Length; i++ )
			{
				Vector3 p = verts[i] * mr.importScale;

				if ( mr.negx )
					p.x = -p.x;

				if ( mr.flipyz )
				{
					float y = p.y;
					p.y = p.z;
					p.z = y;
				}

				verts[i] = p;
			}
		}

		void ConvertPoints(NativeArray<Vector3> verts)
		{
			MegaMorph mr = (MegaMorph)target;

			for ( int i = 0; i < verts.Length; i++ )
			{
				Vector3 p = verts[i] * mr.importScale;

				if ( mr.negx )
					p.x = -p.x;

				if ( mr.flipyz )
				{
					float y = p.y;
					p.y = p.z;
					p.z = y;
				}

				verts[i] = p;
			}
		}

		public bool ParseTarget(BinaryReader br, string id)
		{
			switch ( id )
			{
				case "Name":	currentTarget.name = MegaParse.ReadString(br);	break;
				case "Percent": currentTarget.percent = br.ReadSingle(); break;
				case "TPoints":
					currentTarget.points = new NativeArray<Vector3>(MegaParse.ReadP3v(br), Allocator.Persistent);
					ConvertPoints(currentTarget.points);
					break;
				case "MoPoints":
					Debug.Log("Got morpho points");
					break;
			}

			return true;
		}

		public MegaMorphTarget LoadTarget(BinaryReader br)
		{
			MegaMorphTarget target = new MegaMorphTarget();
			currentTarget = target;

			MegaParse.Parse(br, ParseTarget);
			return target;
		}

		bool TryMapping1(MegaTargetMesh tm, MegaMorph morph)
		{
			MegaModifyObject mod = morph.GetComponent<MegaModifyObject>();

			if ( mod == null )
			{
				EditorUtility.DisplayDialog("Missing ModifyObject!", "No ModifyObject script found on the object", "OK");
				return false;
			}

			int[] mapping = new int[mod.jverts.Length];

			Vector3 min1,max1;
			Vector3 min2,max2;

			Vector3 ex1 = MegaUtils.Extents(mod.jverts, out min1, out max1);
			Vector3 ex2 = MegaUtils.Extents(tm.verts, out min2, out max2);

			float d1 = ex1.x;
			float d2 = ex2.x;

			float scl = d1 / d2;
			bool flipyz = false;
			bool negx = false;

			bool mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);

			if ( !mapped )
			{
				flipyz = true;
				mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);
				if ( !mapped )
				{
					flipyz = false;
					negx = true;
					mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);
					if ( !mapped )
					{
						flipyz = true;
						mapped = DoMapping(mod, morph, tm, mapping, scl, flipyz, negx);
					}
				}
			}

			if ( mapped )
			{
				morph.importScale	= scl;
				morph.flipyz		= flipyz;
				morph.negx			= negx;
				morph.mapping		= mapping;
				morph.oPoints		= new NativeArray<Vector3>(tm.verts.ToArray(), Allocator.Persistent);

				for ( int i = 0; i < morph.oPoints.Length; i++ )
				{
					Vector3 p = morph.oPoints[i];

					if ( negx )
						p.x = -p.x;

					if ( flipyz )
					{
						float z = p.z;
						p.z = p.y;
						p.y = z;
					}

					morph.oPoints[i] = p * morph.importScale;
				}

				return true;
			}
			else
				morph.oPoints = new NativeArray<Vector3>(tm.verts.ToArray(), Allocator.Persistent);

			return false;
		}

		public void OnSceneGUI()
		{
			MegaMorph mod = (MegaMorph)target;
			if ( mod.showmapping )
			{
				if ( mod.oPoints != null )
				{
					float vsize = mod.mappingSize;
					float vsize1 = vsize * 0.75f;
					Matrix4x4 tm = mod.gameObject.transform.localToWorldMatrix;
					Handles.matrix = tm;
					Handles.color = Color.green;

					if ( mo == null )
						mo = mod.gameObject.GetComponent<MegaModifyObject>();

					if ( mo )
					{
						for ( int i = 0; i < mo.jverts.Length; i++ )
						{
							Vector3 p = mo.jverts[i];
							MegaHandles.DotCap(i, p, Quaternion.identity, vsize);
						}
					}

					if ( mod.mapEnd >= mod.oPoints.Length )
						mod.mapEnd = mod.oPoints.Length - 1;

					if ( mod.mapStart > mod.mapEnd )
						mod.mapStart = mod.mapEnd;

					Handles.color = Color.red;

					for ( int i = mod.mapStart; i < mod.mapEnd; i++ )
					{
						Vector3 p = mod.oPoints[i];
						MegaHandles.DotCap(i, p, Quaternion.identity, vsize1);
					}

					Handles.matrix = Matrix4x4.identity;
				}
			}
		}
	}
}
#endif