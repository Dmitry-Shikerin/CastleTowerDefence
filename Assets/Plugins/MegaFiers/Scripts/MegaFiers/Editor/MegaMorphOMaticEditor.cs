using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using Unity.Collections;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaMorphOMatic))]
	public class MegaMorphOMaticEditor : Editor
	{
		Stack<Color>		bcol			= new Stack<Color>();
		Stack<Color>		ccol			= new Stack<Color>();
		Stack<Color>		col				= new Stack<Color>();
		bool				extraparams		= false;
		bool				showmodparams	= false;
		bool				showchannels	= true;
		MegaMorphChan		currentChan;
		MegaMorphTarget		currentTarget;
		static string		lastpath		= " ";
		static public Color	ChanCol1		= new Color(0.44f, 0.67f, 1.0f);
		static public Color	ChanCol2		= new Color(1.0f, 0.67f, 0.44f);

		bool DoMapping(MegaModifyObject mod, MegaMorphOMatic morph, MegaTargetMesh tm, float scale, bool flipyz, bool negx)
		{
			for ( int i = 0; i < mod.jverts.Length; i++ )
			{
				float a = (float)i / (float)mod.jverts.Length;

				EditorUtility.DisplayProgressBar("Mapping", "Mapping vertex " + i, a);
				int map = MegaUtils.FindVert(mod.jverts[i], tm.verts, morph.tolerance, scale, flipyz, negx, i);

				if ( map == -1 )
				{
					EditorUtility.ClearProgressBar();
					return false;
				}
			}

			EditorUtility.ClearProgressBar();
			return true;
		}

		void DisplayTarget(MegaMorphOMatic morph, MegaMorphChan channel, MegaMorphTarget mt, int num)
		{
			PushCols();
			EditorGUI.indentLevel = 1;
			MegaEditorGUILayout.Text(target, "Name", ref mt.name);
			MegaEditorGUILayout.Slider(target, "Percent", ref mt.percent, channel.mSpinmin, channel.mSpinmax);

			EditorGUILayout.BeginHorizontal();

			if ( mt.points == null || mt.points.Length != morph.oPoints.Length )
				GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
			else
				GUI.backgroundColor = new Color(0.0f, 1.0f, 0.0f);

			GUI.backgroundColor = new Color(1.0f, 0.5f, 0.5f);
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

		void DisplayChannel(MegaMorphOMatic morph, MegaMorphChan channel)
		{
			if ( GUILayout.Button(channel.mName) )
				channel.showparams = !channel.showparams;

			GUI.backgroundColor = new Color(1, 1, 1);
			if ( channel.showparams )
			{
				MegaEditorGUILayout.Text(target, "Name", ref channel.mName);

				if ( channel.mTargetCache != null && channel.mTargetCache.Count > 0 )
				{
					MegaEditorGUILayout.Toggle(target, "Active", ref channel.mActiveOverride);
					MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
					MegaEditorGUILayout.Float(target, "Tension", ref channel.mCurvature);
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

				GUI.backgroundColor = new Color(1.5f, 0.5f, 0.5f);
				if ( GUILayout.Button("Delete Channel") )
					morph.chanBank.Remove(channel);

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
					MegaEditorGUILayout.Slider(target, "Percent", ref channel.Percent, channel.mSpinmin, channel.mSpinmax);
			}
		}

		public override void OnInspectorGUI()
		{
			MegaMorphOMatic morph = (MegaMorphOMatic)target;

			PushCols();

			if ( GUILayout.Button("Import MorphOMatic File") )
			{
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

			PopCols();

			if ( GUILayout.Button("Add Channel") )
			{
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

					if ( morph.startchannel < 0 )
						morph.startchannel = 0;

					if ( morph.startchannel >= morph.chanBank.Count - 1 )
						morph.startchannel = morph.chanBank.Count - 1;

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

						DisplayChannel(morph, morph.chanBank[i]);
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

						DisplayChannel(morph, morph.chanBank[i]);
						PopCols();
					}
				}
			}

			extraparams = EditorGUILayout.Foldout(extraparams, "Extra Params");

			if ( extraparams )
			{
				MegaEditorGUILayout.Color(target, "Channel Col 1", ref ChanCol1);
				MegaEditorGUILayout.Color(target, "Channel Col 2", ref ChanCol2);
			}

			PopCols();

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
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
				case "Morph": LoadMorph(br); break;
			}
		}

		void LoadMorph()
		{
			MegaMorphOMatic mr = (MegaMorphOMatic)target;

			string filename = EditorUtility.OpenFilePanel("Morph-O-Matic Morph File", lastpath, "mmf");

			if ( filename == null || filename.Length < 1 )
				return;

			lastpath = filename;

			mr.chanBank.Clear();

			ParseFile(filename, MorphCallback);

			mr.animate = false;
			float looptime = 0.0f;
			for ( int i = 0; i < mr.chanBank.Count; i++ )
			{
				MegaMorphChan mc = mr.chanBank[i];

				if ( mc.control != null )
				{
					mr.animate = true;
					float t = mc.control.Times[mc.control.Times.Length - 1];
					if ( t > looptime )
						looptime = t;
				}
			}

			if ( mr.animate )
				mr.looptime = looptime;

			BuildData();
		}

		public void LoadMorph(BinaryReader br)
		{
			MegaParse.Parse(br, ParseMorph);
		}

		bool AnimCallback(BinaryReader br, string id)
		{
			MegaMorphOMatic mr = (MegaMorphOMatic)target;

			switch ( id )
			{
				case "Chan":
					int cn = br.ReadInt32();
					currentChan = mr.chanBank[cn]; break;
				case "Anim": currentChan.control = LoadAnim(br); break;
				default: return false;
			}

			return true;
		}

		void LoadAnimation(MegaMorphOMatic mr, BinaryReader br)
		{
			MegaParse.Parse(br, AnimCallback);
		}

		public bool ParseMorph(BinaryReader br, string id)
		{
			MegaMorphOMatic mr = (MegaMorphOMatic)target;

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
						EditorUtility.DisplayDialog("Mapping Failed!", "Mapping failed!", "OK");
						EditorUtility.ClearProgressBar();
						return false;
					}
					break;

				case "Channel":		mr.chanBank.Add(LoadChan(br));	break;
				case "Animation":	LoadAnimation(mr, br);			break;
				default: return false;
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

			return chan;
		}

		public static MegaBezFloatKeyControl LoadAnim(BinaryReader br)
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

		Vector3 ConvertPoint(Vector3 v)
		{
			MegaMorphOMatic mr = (MegaMorphOMatic)target;

			Vector3 p = v * mr.importScale;

			if ( mr.negx )
				p.x = -p.x;

			if ( mr.flipyz )
			{
				float y = p.y;
				p.y = p.z;
				p.z = y;
			}

			return p;
		}

		public bool ParseTarget(BinaryReader br, string id)
		{
			switch ( id )
			{
				case "Name":	currentTarget.name		= MegaParse.ReadString(br); break;
				case "Percent": currentTarget.percent	= br.ReadSingle(); break;
				case "MoPoints":
					int count = br.ReadInt32();

					if ( count > 0 )
					{
						currentTarget.loadpoints = new MOPoint[count];

						for ( int i = 0; i < count; i++ )
						{
							MOPoint p = new MOPoint();
							p.id = br.ReadInt32();
							p.p = ConvertPoint(MegaParse.ReadP3(br));

							p.w = br.ReadSingle();
							currentTarget.loadpoints[i] = p;
						}
					}
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

		bool TryMapping1(MegaTargetMesh tm, MegaMorphOMatic morph)
		{
			MegaModifyObject mod = morph.GetComponent<MegaModifyObject>();

			if ( mod == null )
			{
				EditorUtility.DisplayDialog("Missing ModifyObject!", "No ModifyObject script found on the object", "OK");
				return false;
			}

			Vector3 min1,max1;
			Vector3 min2,max2;

			Vector3 ex1 = MegaUtils.Extents(mod.jverts, out min1, out max1);
			Vector3 ex2 = MegaUtils.Extents(tm.verts, out min2, out max2);

			float d1 = ex1.x;
			float d2 = ex2.x;

			float scl = d1 / d2;
			bool flipyz = false;
			bool negx = false;

			bool mapped = DoMapping(mod, morph, tm, scl, flipyz, negx);

			if ( !mapped )
			{
				flipyz = true;
				mapped = DoMapping(mod, morph, tm, scl, flipyz, negx);
				if ( !mapped )
				{
					flipyz = false;
					negx = true;
					mapped = DoMapping(mod, morph, tm, scl, flipyz, negx);
					if ( !mapped )
					{
						flipyz = true;
						mapped = DoMapping(mod, morph, tm, scl, flipyz, negx);
					}
				}
			}

			if ( mapped )
			{
				morph.importScale = scl;
				morph.flipyz = flipyz;
				morph.negx = negx;
				morph.oPoints = tm.verts.ToArray();

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

				morph.mapping = new MegaMomVertMap[morph.oPoints.Length];

				for ( int i = 0; i < morph.oPoints.Length; i++ )
				{
					int[] indices = FindVerts(morph.oPoints[i], mod);

					morph.mapping[i] = new MegaMomVertMap();
					morph.mapping[i].indices = indices;
				}

				return true;
			}

			return false;
		}

		bool GetDelta(MegaMorphTarget targ, int v, out Vector3 delta, out float w)
		{
			MegaMorphOMatic mod = (MegaMorphOMatic)target;

			if ( targ.loadpoints != null )
			{
				for ( int i = 0; i < targ.loadpoints.Length; i++ )
				{
					int id = targ.loadpoints[i].id;
					if ( id == v )
					{
						delta = targ.loadpoints[i].p - mod.oPoints[id];
						w = targ.loadpoints[i].w;
						return true;
					}
				}
			}

			delta = Vector3.zero;
			w = 0.0f;
			return false;
		}

		public int[] FindVerts(Vector3 p, MegaModifyObject mods)
		{
			List<int>	indices = new List<int>();
			for ( int i = 0; i < mods.jverts.Length; i++ )
			{
				float dist = Vector3.Distance(p, mods.jverts[i]);
				if ( dist < 0.0001f )
					indices.Add(i);
			}
			return indices.ToArray();
		}

		public void BuildData()
		{
			MegaMorphOMatic mod = (MegaMorphOMatic)target;

			List<MOMVert>	verts = new List<MOMVert>();

			for ( int c = 0; c < mod.chanBank.Count; c++ )
			{
				MegaMorphChan chan = mod.chanBank[c];

				int maxverts = 0;

				for ( int t = 0; t < chan.mTargetCache.Count - 1; t++ )
				{
					MegaMorphTarget targ = chan.mTargetCache[t];
					MegaMorphTarget targ1 = chan.mTargetCache[t + 1];

					Vector3 delta = Vector3.zero;
					Vector3 delta1 = Vector3.zero;

					float w = 1.0f;

					verts.Clear();

					for ( int v = 0; v < mod.oPoints.Length; v++ )
					{
						bool t1 = GetDelta(targ, v, out delta, out w);
						bool t2 = GetDelta(targ1, v, out delta1, out w);

						if ( t1 || t2 )
						{
							MOMVert vert = new MOMVert();

							vert.id = v;
							vert.w = w;
							vert.start = delta;
							vert.delta = delta1 - delta;

							verts.Add(vert);
						}
					}

					if ( verts.Count > maxverts )
						maxverts = verts.Count;

					if ( verts.Count > 0 )
						targ.mompoints = verts.ToArray();
				}

				for ( int t = 0; t < chan.mTargetCache.Count; t++ )
					chan.mTargetCache[t].loadpoints = null;

				chan.diff = new Vector3[maxverts];
			}
		}
	}
}
#endif