using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Unity.Collections;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPointCache))]
	public class MegaPointCacheEditor : MegaModifierEditor
	{
		struct MCCFrame
		{
			public Vector3[] points;
		}

		public bool			DoAdjustFirst	= false;
		public bool			uselastframe	= false;
		public int			mapframe		= 0;
		MegaModifyObject	mods;
		List<MegaPCVert>	Verts			= new List<MegaPCVert>();
		float				tolerance		= 0.0001f;
		float				scl				= 1.0f;
		bool				flipyz			= false;
		bool				negx			= false;
		bool				negz			= false;
		MegaPCVert[]		oldverts;
		bool				showadvanced	= false;
		MegaModifyObject	mo				= null;
		static string		lastpath		= " ";
		public delegate bool ParseBinCallbackType(BinaryReader br, string id);
		public delegate void ParseClassCallbackType(string classname, BinaryReader br);

		string Read(BinaryReader br, int count)
		{
			byte[] buf = br.ReadBytes(count);
			return System.Text.Encoding.ASCII.GetString(buf, 0, buf.Length);
		}

		void LoadMCC()
		{
			string filename = EditorUtility.OpenFilePanel("Maya Cache File", lastpath, "mc");

			if ( filename == null || filename.Length < 1 )
				return;

			LoadMCC(filename);
		}

		public void LoadMCC(string filename)
		{
			MegaPointCache am = (MegaPointCache)target;

			if ( !am.cacheFile )
				am.cacheFile = CreateCacheFile(filename);

			oldverts = am.cacheFile.Verts;
			mods = am.gameObject.GetComponent<MegaModifyObject>();

			if ( mods == null )
			{
				Debug.LogWarning("You need to add a Mega Modify Object component first!");
				return;
			}

			lastpath = filename;

			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);

			BinaryReader br = new BinaryReader(fs);

			string id = Read(br, 4);
			if ( id != "FOR4" )
			{
				Debug.Log("wrong file");
				return;
			}

			int offset  = MegaParse.ReadMotInt(br);

			br.ReadBytes(offset);

			List<MCCFrame> frames = new List<MCCFrame>();

			int buflen = 0;

			Matrix4x4 tm = Matrix4x4.TRS(am.mapPos, Quaternion.Euler(am.mapRot), am.mapScl);
			am.importPos = am.mapPos;
			am.importRot = am.mapRot;
			am.importScl = am.mapScl;

			while ( true )
			{
				string btag = Read(br, 4);
				if ( btag == "" )
					break;

				if ( btag != "FOR4" )
				{
					Debug.Log("File format error");
					return;
				}

				int blocksize = MegaParse.ReadMotInt(br);

				int bytesread = 0;

				btag = Read(br, 4);
				if ( btag != "MYCH" )
				{
					Debug.Log("File format error");
					return;
				}
				bytesread += 4;

				btag = Read(br, 4);
				if ( btag != "TIME" )
				{
					Debug.Log("File format error");
					return;
				}
				bytesread += 4;

				br.ReadBytes(4);
				bytesread += 4;

				int time = MegaParse.ReadMotInt(br);
				bytesread += 4;

				am.maxtime = (float)time / 6000.0f;

				Vector3 p = Vector3.zero;

				while ( bytesread < blocksize )
				{
					btag = Read(br, 4);
					if ( btag != "CHNM" )
					{
						Debug.Log("chm error");
						return;
					}
					bytesread += 4;

					int chnmsize = MegaParse.ReadMotInt(br);
					bytesread += 4;

					int mask = 3;
					int chnmsizetoread = (chnmsize + mask) & (~mask);
					br.ReadBytes(chnmsize);

					int paddingsize = chnmsizetoread - chnmsize;

					if ( paddingsize > 0 )
						br.ReadBytes(paddingsize);

					bytesread += chnmsizetoread;

					btag = Read(br, 4);

					if ( btag != "SIZE" )
					{
						Debug.Log("Size error");
						return;
					}
					bytesread += 4;

					br.ReadBytes(4);
					bytesread += 4;

					int arraylength = MegaParse.ReadMotInt(br);
					bytesread += 4;

					MCCFrame frame = new MCCFrame();
					frame.points = new Vector3[arraylength];

					string dataformattag = Read(br, 4);
					int bufferlength = MegaParse.ReadMotInt(br);

					if ( buflen == 0 )
					{
						buflen = bufferlength;
					}

					bytesread += 8;

					if ( dataformattag == "FVCA" )
					{
						if ( bufferlength != arraylength * 3 * 4 )
						{
							Debug.Log("buffer len error");
							return;
						}

						for ( int i = 0; i < arraylength; i++ )
						{
							p.x = MegaParse.ReadMotFloat(br);
							p.y = MegaParse.ReadMotFloat(br);
							p.z = MegaParse.ReadMotFloat(br);

							frame.points[i] = tm.MultiplyPoint(p);  //MegaParse.ReadMotFloat(br);

							//frame.points[i].x = MegaParse.ReadMotFloat(br);
							//frame.points[i].y = MegaParse.ReadMotFloat(br);
							//frame.points[i].z = MegaParse.ReadMotFloat(br);
						}

						bytesread += arraylength * 3 * 4;
					}
					else
					{
						if ( dataformattag == "DVCA" )
						{
							if ( bufferlength != arraylength * 3 * 8 )
							{
								Debug.Log("buffer len error");
								return;
							}

							for ( int i = 0; i < arraylength; i++ )
							{
								p.x = MegaParse.ReadMotFloat(br);
								p.y = MegaParse.ReadMotFloat(br);
								p.z = MegaParse.ReadMotFloat(br);

								frame.points[i] = tm.MultiplyPoint(p);  //MegaParse.ReadMotFloat(br);

								//frame.points[i].x = (float)MegaParse.ReadMotDouble(br);
								//frame.points[i].y = (float)MegaParse.ReadMotDouble(br);
								//frame.points[i].z = (float)MegaParse.ReadMotDouble(br);
							}

							bytesread += arraylength * 3 * 8;
						}
						else
						{
							Debug.Log("Format Error");
							return;
						}
					}

					if ( buflen == bufferlength )
						frames.Add(frame);
				}
			}

			am.cacheFile.Verts = new MegaPCVert[frames[0].points.Length];

			for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
			{
				am.cacheFile.Verts[i] = new MegaPCVert();
				am.cacheFile.Verts[i].points = new Vector3[frames.Count];

				for ( int p = 0; p < am.cacheFile.Verts[i].points.Length; p++ )
					am.cacheFile.Verts[i].points[p] = frames[p].points[i];
			}

			BuildData(mods, am, filename);
			br.Close();
			AssetDatabase.Refresh();
		}

		void LoadMDD()
		{
			string filename = EditorUtility.OpenFilePanel("Motion Designer File", lastpath, "mdd");

			if ( filename == null || filename.Length < 1 )
				return;

			LoadMDD(filename);
		}

		MegaPointCacheFile CreateCacheFile(string filename)
		{
			MegaPointCache mod = (MegaPointCache)target;

			MegaPointCacheFile cacheFile = ScriptableObject.CreateInstance<MegaPointCacheFile>();

			string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(mod.gameObject);

			path = Path.GetFileNameWithoutExtension(path);

			path = path + " Cache.asset";
			Debug.Log("p " + path);

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + path);

			Debug.Log("path " + assetPathAndName);

			AssetDatabase.CreateAsset(cacheFile, assetPathAndName);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			return cacheFile;
		}

		public void LoadMDD(string filename)
		{
			MegaPointCache am = (MegaPointCache)target;

			if ( !am.cacheFile )
				am.cacheFile = CreateCacheFile(filename);

			oldverts = am.cacheFile.Verts;
			mods = am.gameObject.GetComponent<MegaModifyObject>();

			if ( mods == null)
			{
				Debug.LogWarning("You need to add a Mega Modify Object component first!");
				return;
			}
			lastpath = filename;

			Verts.Clear();

			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);

			BinaryReader br = new BinaryReader(fs);

			int numSamples = MegaParse.ReadMotInt(br);
			int numPoints = MegaParse.ReadMotInt(br);

			float t = 0.0f;

			for ( int i = 0; i < numSamples; i++ )
				t = MegaParse.ReadMotFloat(br);

			Matrix4x4 tm = Matrix4x4.TRS(am.mapPos, Quaternion.Euler(am.mapRot), am.mapScl);
			am.importPos = am.mapPos;
			am.importRot = am.mapRot;
			am.importScl = am.mapScl;

			am.maxtime = t;

			am.cacheFile.Verts = new MegaPCVert[numPoints];

			for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
			{
				am.cacheFile.Verts[i] = new MegaPCVert();
				am.cacheFile.Verts[i].points = new Vector3[numSamples];
			}

			Vector3 p = Vector3.zero;

			for ( int i = 0; i < numSamples; i++ )
			{
				for ( int v = 0; v < numPoints; v++ )
				{
					p.x = MegaParse.ReadMotFloat(br);
					p.y = MegaParse.ReadMotFloat(br);
					p.z = MegaParse.ReadMotFloat(br);

					am.cacheFile.Verts[v].points[i] = tm.MultiplyPoint(p);	//MegaParse.ReadMotFloat(br);
					//am.Verts[v].points[i].y = MegaParse.ReadMotFloat(br);
					//am.Verts[v].points[i].z = MegaParse.ReadMotFloat(br);
				}
			}

			BuildData(mods, am, filename);
			br.Close();
			AssetDatabase.Refresh();
		}

		void BuildData(MegaModifyObject mods, MegaPointCache am, string filename)
		{
			bool domapping = true;
			if ( am.havemapping )
				domapping = EditorUtility.DisplayDialog("Point Cache Mapping", "Replace Existing Mapping?", "Yes", "No");

			if ( !domapping )
			{
				if ( DoAdjustFirst )
					AdjustVertsSimple(mods, am);
			}
			else
			{
				if ( DoAdjustFirst )
					AdjustVerts(mods, am);
			}
			Vector3[] baseverts = new Vector3[am.cacheFile.Verts.Length];

			int findex = 0;
			if ( uselastframe )
			{
				findex = am.cacheFile.Verts[0].points.Length - 1;
			}
			for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
				baseverts[i] = am.cacheFile.Verts[i].points[findex];

			if ( domapping )
			{
				if ( !TryMapping1(baseverts, mods.jverts) )
				{
					EditorUtility.DisplayDialog("Mapping Failed!", "Mapping of " + System.IO.Path.GetFileNameWithoutExtension(filename) + " failed!", "OK");
					EditorUtility.ClearProgressBar();
					am.havemapping = false;

					EditorUtility.SetDirty(am.cacheFile);
					AssetDatabase.SaveAssets();
					return;
				}

				am.negx = negx;
				am.negz = negz;
				am.flipyz = flipyz;
				am.scl = scl;
			}
			else
			{
				negx = am.negx;
				negz = am.negz;
				flipyz = am.flipyz;
				scl = am.scl;
			}

			am.havemapping = true;

			for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
			{
				for ( int v = 0; v < am.cacheFile.Verts[i].points.Length; v++ )
				{
					if ( negx )
						am.cacheFile.Verts[i].points[v].x = -am.cacheFile.Verts[i].points[v].x;

					if ( flipyz )
					{
						float z = am.cacheFile.Verts[i].points[v].z;
						am.cacheFile.Verts[i].points[v].z = am.cacheFile.Verts[i].points[v].y;
						am.cacheFile.Verts[i].points[v].y = z;
					}

					if ( negz )
						am.cacheFile.Verts[i].points[v].z = -am.cacheFile.Verts[i].points[v].z;

					am.cacheFile.Verts[i].points[v] = am.cacheFile.Verts[i].points[v] * scl;
				}
			}

			if ( domapping )
			{
				for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
				{
					am.cacheFile.Verts[i].indices = FindVerts(am.cacheFile.Verts[i].points[findex]);

					if ( am.cacheFile.Verts[i].indices.Length == 0 )
					{
						EditorUtility.DisplayDialog("Final Mapping Failed!", "Mapping of " + System.IO.Path.GetFileNameWithoutExtension(filename) + " failed!", "OK");
						EditorUtility.ClearProgressBar();
						return;
					}
				}
			}
			else
			{
				for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
					am.cacheFile.Verts[i].indices = oldverts[i].indices;
			}

			oldverts = null;

			am.cacheFile.cacheValues = new Vector3[mods.jverts.Length * am.cacheFile.Verts[0].points.Length];

			for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
			{
				for ( int v = 0; v < am.cacheFile.Verts[i].indices.Length; v++ )
				{
					for ( int f = 0; f < am.cacheFile.Verts[0].points.Length; f++ )
						am.cacheFile.cacheValues[(f * mods.jverts.Length) + am.cacheFile.Verts[i].indices[v]] = am.cacheFile.Verts[i].points[f];
				}
			}

			EditorUtility.SetDirty(am.cacheFile);
			AssetDatabase.SaveAssets();
		}

		public void LoadFile(string filename)
		{
			string ext = System.IO.Path.GetExtension(filename);

			ext = ext.ToLower();

			switch ( ext )
			{
				case ".pc2":
					LoadPC2(filename);
					break;

				case ".mdd":
					LoadMDD(filename);
					break;

				case ".mc":
					LoadMCC(filename);
					break;
			}
		}

		void LoadPC2()
		{
			string filename = EditorUtility.OpenFilePanel("Point Cache File", lastpath, "pc2");

			if ( filename == null || filename.Length < 1 )
				return;

			LoadPC2(filename);
		}

		public void LoadPC2(string filename)
		{
			MegaPointCache am = (MegaPointCache)target;

			if ( !am.cacheFile )
				am.cacheFile = CreateCacheFile(filename);

			oldverts = am.cacheFile.Verts;
			mods = am.gameObject.GetComponent<MegaModifyObject>();

			if ( mods == null )
			{
				Debug.LogWarning("You need to add a Mega Modify Object component first!");
				return;
			}

			lastpath = filename;

			Verts.Clear();

			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);

			long len = fs.Length;
			BinaryReader br = new BinaryReader(fs);

			string sig = MegaParse.ReadStr(br);
			if ( sig != "POINTCACHE2" )
			{
				EditorUtility.DisplayDialog("PC2 Importer", "The selected file does not appear to be a valid PC2 File", "Ok");
				br.Close();
				return;
			}

			int fileVersion = br.ReadInt32();
			if ( fileVersion != 1 )
			{
				br.Close();
				return;
			}

			int numPoints = br.ReadInt32();
			br.ReadSingle();
			br.ReadSingle();
			int numSamples = br.ReadInt32();
			long csamples = (len - 24) / (numPoints * 12);
			numSamples = (int)csamples;

			am.cacheFile.Verts = new MegaPCVert[numPoints];

			Matrix4x4 tm = Matrix4x4.TRS(am.mapPos, Quaternion.Euler(am.mapRot), am.mapScl);
			am.importPos = am.mapPos;
			am.importRot = am.mapRot;
			am.importScl = am.mapScl;

			for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
			{
				am.cacheFile.Verts[i] = new MegaPCVert();
				am.cacheFile.Verts[i].points = new Vector3[numSamples];
			}

			for ( int i = 0; i < numSamples; i++ )
			{
				for ( int v = 0; v < numPoints; v++ )
					am.cacheFile.Verts[v].points[i] = tm.MultiplyPoint(MegaParse.ReadP3(br));
			}
			BuildData(mods, am, filename);
			br.Close();

			AssetDatabase.Refresh();
		}

		int[] FindVerts(Vector3 p)
		{
			List<int>	indices = new List<int>();
			for ( int i = 0; i < mods.jverts.Length; i++ )
			{
				float dist = Vector3.SqrMagnitude(p - mods.jverts[i]);
				if ( dist < tolerance )
					indices.Add(i);
			}
			return indices.ToArray();
		}

		Vector3 Extents(MegaPCVert[] verts, out Vector3 min, out Vector3 max)
		{
			Vector3 extent = Vector3.zero;

			min = Vector3.zero;
			max = Vector3.zero;

			if ( verts != null && verts.Length > 0 )
			{
				min = verts[0].points[0];
				max = verts[0].points[0];

				for ( int i = 1; i < verts.Length; i++ )
				{
					Vector3 p = verts[i].points[0];

					if ( p.x < min.x ) min.x = p.x;
					if ( p.y < min.y ) min.y = p.y;
					if ( p.z < min.z ) min.z = p.z;

					if ( p.x > max.x ) max.x = p.x;
					if ( p.y > max.y ) max.y = p.y;
					if ( p.z > max.z ) max.z = p.z;
				}

				extent = max - min;
			}

			return extent;
		}

		public override void OnInspectorGUI()
		{
			MegaPointCache am = (MegaPointCache)target;

			DoAdjustFirst = EditorGUILayout.Toggle("Mapping Adjust", DoAdjustFirst);
			uselastframe = EditorGUILayout.Toggle("Use Last Frame", uselastframe);

			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("Import PC2") )
			{
				LoadPC2();
				EditorUtility.SetDirty(target);
			}

			if ( GUILayout.Button("Import MDD") )
			{
				LoadMDD();
				EditorUtility.SetDirty(target);
			}

			if ( GUILayout.Button("Import MC") )
			{
				LoadMCC();
				EditorUtility.SetDirty(target);
			}

			EditorGUILayout.EndHorizontal();

			if ( GUILayout.Button("Delete Mapping") )
			{
				if ( am.cacheFile )
					MegaPointCacheFileEditor.DeleteMapping(am.cacheFile);
			}

			am.showModParams = EditorGUILayout.Foldout(am.showModParams, "Modifier Common Params");

			if ( am.showModParams )
				CommonModParamsBasic(am);

			showadvanced = EditorGUILayout.Foldout(showadvanced, "Advanced Params");

			if ( showadvanced )
			{
				tolerance = EditorGUILayout.FloatField("Map Tolerance", tolerance * 100.0f) / 100.0f;
				if ( am.cacheFile && am.cacheFile.Verts != null && am.cacheFile.Verts.Length > 0 )
				{
					MegaEditorGUILayout.BeginToggle(target, "Show Mapping", ref am.showmapping);
					MegaEditorGUILayout.Int(target, "Every nth Vert", ref am.everyNth);
					am.everyNth = Mathf.Clamp(am.everyNth, 1, 32);
					MegaEditorGUILayout.Vector3(target, "Map Pos", ref am.mapPos);
					MegaEditorGUILayout.Vector3(target, "Map Rot", ref am.mapRot);
					MegaEditorGUILayout.Vector3(target, "Map Scl", ref am.mapScl);
					MegaEditorGUILayout.Int(target, "StartVert", ref am.mapStart, 0, am.cacheFile.Verts.Length);
					MegaEditorGUILayout.Int(target, "endVert", ref am.mapEnd, 0, am.cacheFile.Verts.Length);
					MegaEditorGUILayout.Slider(target, "Size", ref am.mappingSize, 0.0005f, 10.01f);
					MegaEditorGUILayout.EndToggle();
				}
			}

			am.cacheFile = (MegaPointCacheFile)EditorGUILayout.ObjectField("Cache File", am.cacheFile, typeof(MegaPointCacheFile), false);
			MegaEditorGUILayout.Float(target, "Time", ref am.time);
			MegaEditorGUILayout.Float(target, "Loop Time", ref am.maxtime);
			MegaEditorGUILayout.Toggle(target, "Animated", ref am.animated);
			MegaEditorGUILayout.Toggle(target, "Frame Delay", ref am.framedelay);
			MegaEditorGUILayout.Float(target, "Speed", ref am.speed);
			MegaEditorGUILayout.RepeatMode(target, "Loop Mode", ref am.LoopMode);
			MegaEditorGUILayout.InterpMethod(target, "Interp Method", ref am.interpMethod);

			MegaEditorGUILayout.BlendMode(target, "Blend Mode", ref am.blendMode);
			if ( am.blendMode == MegaBlendAnimMode.Additive )
				MegaEditorGUILayout.Float(target, "Weight", ref am.weight);

			if ( am.cacheFile && am.cacheFile.Verts != null && am.cacheFile.Verts.Length > 0 )
			{
				int mem = am.cacheFile.Verts.Length * am.cacheFile.Verts[0].points.Length * 12;
				EditorGUILayout.LabelField("Memory: ", (mem / 1024) + "KB");
			}

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}

		int FindVert(Vector3 vert, Vector3[] tverts, float tolerance, float scl, bool flipyz, bool negx, bool negz, int vn)
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

			if ( negz )
				vert.z = -vert.z;

			vert /= scl;

			float closest = Vector3.SqrMagnitude(tverts[0] - vert);

			for ( int i = 0; i < tverts.Length; i++ )
			{
				float dif = Vector3.SqrMagnitude(tverts[i] - vert);

				if ( dif < closest )
				{
					closest = dif;
					find = i;
				}
			}

			if ( closest > tolerance )
			{
				return -1;
			}

			return find;
		}

		bool DoMapping(Vector3[] verts, Vector3[] tverts, float scale, bool flipyz, bool negx, bool negz)
		{
			int count = 400;

			for ( int i = 0; i < verts.Length; i++ )
			{
				float a = (float)i / (float)verts.Length;

				count--;
				if ( count < 0 )
				{
					EditorUtility.DisplayProgressBar("Mapping", "Mapping vertex " + i, a);
					count = 400;
				}
				int mapping = FindVert(verts[i], tverts, tolerance, scale, flipyz, negx, negz, i);

				if ( mapping == -1 )
				{
					EditorUtility.ClearProgressBar();
					return false;
				}
			}

			EditorUtility.ClearProgressBar();
			return true;
		}

		bool DoMapping(NativeArray<Vector3> verts, Vector3[] tverts, float scale, bool flipyz, bool negx, bool negz)
		{
			int count = 400;

			for ( int i = 0; i < verts.Length; i++ )
			{
				float a = (float)i / (float)verts.Length;

				count--;
				if ( count < 0 )
				{
					EditorUtility.DisplayProgressBar("Mapping", "Mapping vertex " + i, a);
					count = 400;
				}
				int mapping = FindVert(verts[i], tverts, tolerance, scale, flipyz, negx, negz, i);

				if ( mapping == -1 )
				{
					EditorUtility.ClearProgressBar();
					return false;
				}
			}

			EditorUtility.ClearProgressBar();
			return true;
		}

		bool TryMapping1(Vector3[] tverts, Vector3[] verts)
		{
			Vector3 min1,max1;
			Vector3 min2,max2;

			Vector3 ex1 = MegaUtils.Extents(verts, out min1, out max1);
			Vector3 ex2 = MegaUtils.Extents(tverts, out min2, out max2);

			int largest1 = MegaUtils.LargestComponent(ex1);
			int largest2 = MegaUtils.LargestComponent(ex2);

			scl = ex1[largest1] / ex2[largest2];

			int map = 0;

			for ( map = 0; map < 8; map++ )
			{
				flipyz = ((map & 4) != 0);
				negx = ((map & 2) != 0);
				negz = ((map & 1) != 0);

				bool mapped = DoMapping(verts, tverts, scl, flipyz, negx, negz);
				if ( mapped )
					break;
			}

			if ( map == 8 )
				return false;

			return true;
		}

		bool TryMapping1(Vector3[] tverts, NativeArray<Vector3> verts)
		{
			Vector3 min1, max1;
			Vector3 min2, max2;

			Vector3 ex1 = MegaUtils.Extents(verts, out min1, out max1);
			Vector3 ex2 = MegaUtils.Extents(tverts, out min2, out max2);

			int largest1 = MegaUtils.LargestComponent(ex1);
			int largest2 = MegaUtils.LargestComponent(ex2);

			scl = ex1[largest1] / ex2[largest2];

			int map = 0;

			for ( map = 0; map < 8; map++ )
			{
				flipyz = ((map & 4) != 0);
				negx = ((map & 2) != 0);
				negz = ((map & 1) != 0);

				bool mapped = DoMapping(verts, tverts, scl, flipyz, negx, negz);
				if ( mapped )
					break;
			}

			if ( map == 8 )
				return false;

			return true;
		}

		void AdjustVertsSimple(MegaModifyObject mods, MegaPointCache am)
		{
			if ( am.cacheFile.Verts != null )
			{
				Vector3[] baseverts = new Vector3[am.cacheFile.Verts.Length];

				for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
					baseverts[i] = am.cacheFile.Verts[i].points[0];

				for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
				{
					for ( int j = 0; j < am.cacheFile.Verts[i].points.Length; j++ )
					{
						Vector3 p = am.cacheFile.Verts[i].points[j] * am.adjustscl;

						am.cacheFile.Verts[i].points[j] = p - am.adjustoff;
					}
				}
			}
		}

		void AdjustVerts(MegaModifyObject mods, MegaPointCache am)
		{
			if ( am.cacheFile.Verts != null )
			{
				Vector3[] baseverts = new Vector3[am.cacheFile.Verts.Length];

				for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
					baseverts[i] = am.cacheFile.Verts[i].points[0];

				Vector3 min1,max1;
				Vector3 min2,max2;

				Vector3 ex1 = MegaUtils.Extents(mods.jverts, out min1, out max1);
				Vector3 ex2 = MegaUtils.Extents(baseverts, out min2, out max2);

				int largest1 = MegaUtils.LargestComponent(ex1);
				int largest2 = MegaUtils.LargestComponent(ex2);

				am.adjustscl = ex1[largest1] / ex2[largest2];

				am.adjustoff = (min2 * am.adjustscl) - min1;

				for ( int i = 0; i < am.cacheFile.Verts.Length; i++ )
				{
					for ( int j = 0; j < am.cacheFile.Verts[i].points.Length; j++ )
					{
						Vector3 p = am.cacheFile.Verts[i].points[j] * am.adjustscl;

						am.cacheFile.Verts[i].points[j] = p - am.adjustoff;
					}
				}
			}
		}

		public override void DrawSceneGUI()
		{
			MegaPointCache mod = (MegaPointCache)target;
			if ( mod.showmapping )
			{
				if ( mod.cacheFile && mod.cacheFile.Verts != null && mod.cacheFile.Verts[0] != null )
				{
					float vsize = mod.mappingSize;
					float vsize1 = vsize * 0.75f;
					Matrix4x4 tm = mod.gameObject.transform.localToWorldMatrix;	// * maptm;
					Handles.matrix = tm;
					Handles.color = Color.green;

					if ( mo == null )
						mo = mod.gameObject.GetComponent<MegaModifyObject>();

					if ( mo )
					{
						for ( int i = 0; i < mo.jverts.Length; i += mod.everyNth )
						{
							Vector3 p = mo.jverts[i];
							MegaHandles.DotCap(i, p, Quaternion.identity, vsize);
						}
					}

					if ( mod.mapEnd >= mod.cacheFile.Verts.Length )
						mod.mapEnd = mod.cacheFile.Verts.Length - 1;

					if ( mod.mapStart > mod.mapEnd )
						mod.mapStart = mod.mapEnd;

					Handles.color = Color.white;

					Matrix4x4 impmaptm = Matrix4x4.TRS(mod.importPos, Quaternion.Euler(mod.importRot), mod.importScl);
					Matrix4x4 invimptm = impmaptm.inverse;

					Matrix4x4 maptm = Matrix4x4.TRS(mod.mapPos, Quaternion.Euler(mod.mapRot), mod.mapScl);
					tm = mod.gameObject.transform.localToWorldMatrix * maptm;
					Handles.matrix = tm;

					int findex = 0;
					if ( uselastframe )
					{
						findex = mod.cacheFile.Verts[0].points.Length - 1;
					}
					for ( int i = mod.mapStart; i < mod.mapEnd; i += mod.everyNth )
					{
						Vector3 p = mod.cacheFile.Verts[i].points[findex];
						p = invimptm.MultiplyPoint(p);	// back to original

						MegaHandles.DotCap(i, p, Quaternion.identity, vsize1);
					}

					Handles.matrix = Matrix4x4.identity;
				}
			}
		}
	}
}