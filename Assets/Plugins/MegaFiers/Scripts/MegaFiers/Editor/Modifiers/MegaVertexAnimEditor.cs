using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaVertexAnim))]
	public class MegaVertexAnimEditor : MegaModifierEditor
	{
		MegaModifyObject			mods;
		List<MegaAnimatedVert>	Verts		= new List<MegaAnimatedVert>();
		MegaAnimatedVert		currentVert;
		float					scl			= 1.0f;
		static string			lastpath	= " ";
		public delegate bool ParseBinCallbackType(BinaryReader br, string id);
		public delegate void ParseClassCallbackType(string classname, BinaryReader br);

		void LoadVertexAnim()
		{
			MegaVertexAnim am = (MegaVertexAnim)target;
			mods = am.gameObject.GetComponent<MegaModifyObject>();

			string filename = EditorUtility.OpenFilePanel("Vertex Animation File", lastpath, "mpc");

			if ( filename == null || filename.Length < 1 )
				return;

			lastpath = filename;

			Verts.Clear();

			ParseFile(filename, AnimatedMeshCallback);
			am.Verts = Verts.ToArray();

			BitArray animated = new BitArray(mods.jverts.Length);
			int count = 0;
			for ( int i = 0; i < Verts.Count; i++ )
			{
				for ( int v = 0; v < Verts[i].indices.Length; v++ )
				{
					if ( !animated[Verts[i].indices[v]] )
					{
						animated[Verts[i].indices[v]] = true;
						count++;
					}
				}
			}

			am.NoAnim = new int[mods.jverts.Length - count];
			int index = 0;
			for ( int i = 0; i < animated.Count; i++ )
			{
				if ( !animated[i] )
					am.NoAnim[index++] = i;
			}

			am.maxtime = 0.0f;
			for ( int i = 0; i < Verts.Count; i++ )
			{
				float t = Verts[i].con.Times[Verts[i].con.Times.Length - 1];
				if ( t > am.maxtime )
						am.maxtime = t;
			}
		}

		void AnimatedMeshCallback(string classname, BinaryReader br)
		{
			switch ( classname )
			{
				case "AnimMesh": LoadAnimMesh(br); break;
			}
		}

		public void LoadAnimMesh(BinaryReader br)
		{
			MegaParse.Parse(br, ParseAnimMesh);
		}

		int[] FindVerts(Vector3 p)
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

		public bool ParseAnimMesh(BinaryReader br, string id)
		{
			switch ( id )
			{
				case "Size":
					Vector3 sz = MegaParse.ReadP3(br);
					Vector3 min1,max1;

					Vector3 ex1 = MegaUtils.Extents(mods.jverts, out min1, out max1);

					int largest = 0;
					if ( sz.x > sz.y )
					{
						if ( sz.x > sz.z )
							largest = 0;
						else
							largest = 2;
					}
					else
					{
						if ( sz.y > sz.z )
							largest = 1;
						else
							largest = 2;
					}

					scl = ex1[largest] / sz[largest];
					break;

				case "V":
					Vector3 p = MegaParse.ReadP3(br) * scl;
					currentVert = new MegaAnimatedVert();
					currentVert.startVert = p;
					currentVert.indices = FindVerts(p);
					if ( currentVert.indices == null )
						Debug.Log("Error! No match found");

					Verts.Add(currentVert);
					break;

				case "Anim":
					currentVert.con = MegaParseBezVector3Control.LoadBezVector3KeyControl(br);
					currentVert.con.Scale(scl);
					break;

				default: return false;
			}

			return true;
		}

		public void ParseFile(string assetpath, ParseClassCallbackType cb)
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

		public override void OnInspectorGUI()
		{
			MegaVertexAnim am = (MegaVertexAnim)target;

			if ( GUILayout.Button("Import Vertex Anim File") )
			{
				LoadVertexAnim();
				EditorUtility.SetDirty(target);
			}

			am.showModParams = EditorGUILayout.Foldout(am.showModParams, "Modifier Common Params");

			if ( am.showModParams )
				CommonModParamsBasic(am);

			MegaEditorGUILayout.Float(target, "Time", ref am.time);
			MegaEditorGUILayout.Float(target, "Loop Time", ref am.maxtime);
			MegaEditorGUILayout.Toggle(target, "Animated", ref am.animated);
			MegaEditorGUILayout.Float(target, "Speed", ref am.speed);
			MegaEditorGUILayout.RepeatMode(target, "Loop Mode", ref am.LoopMode);
			MegaEditorGUILayout.BlendMode(target, "Blend Mode", ref am.blendMode);

			if ( am.blendMode == MegaBlendAnimMode.Additive )
				MegaEditorGUILayout.Float(target, "Weight", ref am.weight);

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}
	}
}