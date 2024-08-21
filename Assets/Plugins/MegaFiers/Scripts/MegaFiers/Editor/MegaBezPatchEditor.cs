using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaBezPatch))]
	public class MegaBezPatchEditor : Editor
	{
		[MenuItem("GameObject/Create Other/MegaShape/Bez Patch")]
		static void CreateBezPatch()
		{
			Vector3 pos = Vector3.zero;
			if ( UnityEditor.SceneView.lastActiveSceneView != null )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject("Bez Patch");

			MeshFilter mf = go.AddComponent<MeshFilter>();
			mf.sharedMesh = new Mesh();
			MeshRenderer mr = go.AddComponent<MeshRenderer>();

			Material[] mats = new Material[1];

			mr.sharedMaterials = mats;
			MegaBezPatch pm = go.AddComponent<MegaBezPatch>();
			pm.mesh = mf.sharedMesh;
			go.transform.position = pos;
			Selection.activeObject = go;
			pm.Rebuild();
		}

		public override void OnInspectorGUI()
		{
			MegaBezPatch mod = (MegaBezPatch)target;

			float Width = mod.Width;
			MegaEditorGUILayout.Float(target, "Width", ref mod.Width);
			if ( Width != mod.Width )
				mod.AdjustLattice(mod.Width, mod.Height);

			float Height = mod.Height;
			MegaEditorGUILayout.Float(target, "Height", ref mod.Height);
			if ( Height != mod.Height )
				mod.AdjustLattice(mod.Width, mod.Height);

			MegaEditorGUILayout.Int(target, "Width Segs", ref mod.WidthSegs);
			MegaEditorGUILayout.Int(target, "Height Segs", ref mod.HeightSegs);

			MegaEditorGUILayout.Toggle(target, "Recalc Bounds", ref mod.recalcBounds);
			MegaEditorGUILayout.Toggle(target, "Recalc Tangents", ref mod.recalcTangents);

			MegaEditorGUILayout.BeginToggle(target, "Gen UVs", ref mod.GenUVs);
			MegaEditorGUILayout.Vector2(target, "UV Offset", ref mod.UVOffset);
			MegaEditorGUILayout.Vector2(target, "UV Scale", ref mod.UVScale);
			EditorGUILayout.EndToggleGroup();

			MegaEditorGUILayout.Toggle(target, "Show Gizmos", ref mod.showgizmos);
			MegaEditorGUILayout.Toggle(target, "Show Labels", ref mod.showlabels);
			MegaEditorGUILayout.Color(target, "Lattice Color", ref mod.latticecol);
			MegaEditorGUILayout.Toggle(target, "Position Handles", ref mod.positionhandles);
			MegaEditorGUILayout.Float(target, "Handle Size", ref mod.handlesize);
			MegaEditorGUILayout.Vector2(target, "Snap", ref mod.snap);
			MegaEditorGUILayout.Toggle(target, "Align", ref mod.align);

			mod.showlatticepoints = EditorGUILayout.Foldout(mod.showlatticepoints, "Lattice Points");

			MegaEditorGUILayout.Float(target, "Switch Time", ref mod.switchtime);
			MegaEditorGUILayout.Toggle(target, "Animate Warps", ref mod.animateWarps);

			if ( mod.showlatticepoints )
			{
				MegaEditorGUILayout.Vector3Field2(target, "p11", ref mod.p11);
				MegaEditorGUILayout.Vector3Field2(target, "p12", ref mod.p12);
				MegaEditorGUILayout.Vector3Field2(target, "p13", ref mod.p13);
				MegaEditorGUILayout.Vector3Field2(target, "p14", ref mod.p14);

				MegaEditorGUILayout.Vector3Field2(target, "p21", ref mod.p21);
				MegaEditorGUILayout.Vector3Field2(target, "p22", ref mod.p22);
				MegaEditorGUILayout.Vector3Field2(target, "p23", ref mod.p23);
				MegaEditorGUILayout.Vector3Field2(target, "p24", ref mod.p24);

				MegaEditorGUILayout.Vector3Field2(target, "p31", ref mod.p31);
				MegaEditorGUILayout.Vector3Field2(target, "p32", ref mod.p32);
				MegaEditorGUILayout.Vector3Field2(target, "p33", ref mod.p33);
				MegaEditorGUILayout.Vector3Field2(target, "p34", ref mod.p34);

				MegaEditorGUILayout.Vector3Field2(target, "p41", ref mod.p41);
				MegaEditorGUILayout.Vector3Field2(target, "p42", ref mod.p42);
				MegaEditorGUILayout.Vector3Field2(target, "p43", ref mod.p43);
				MegaEditorGUILayout.Vector3Field2(target, "p44", ref mod.p44);
			}

			MegaEditorGUILayout.Int(target, "Dest Warp", ref mod.destwarp);
			int currentwarp = mod.currentwarp;
			MegaEditorGUILayout.Int(target, "Warp", ref mod.currentwarp, 0, mod.warps.Count - 1);
			if ( currentwarp != mod.currentwarp )
				mod.SetWarp(mod.currentwarp);

			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("Add Warp") )
			{
				Undo.RecordObject(target, "Add Warp");
				mod.AddWarp();
			}

			if ( GUILayout.Button("Reset") )
			{
				Undo.RecordObject(target, "Reset");
				mod.Reset();
			}

			EditorGUILayout.EndHorizontal();

			for ( int i = 0; i < mod.warps.Count; i++ )
			{
				EditorGUILayout.BeginHorizontal();
				MegaEditorGUILayout.Text(target, "", ref mod.warps[i].name);

				if ( GUILayout.Button("Set", GUILayout.MaxWidth(50)) )
				{
					Undo.RecordObject(target, "Set");
					mod.SetWarp(i);
					EditorUtility.SetDirty(mod);
				}

				if ( GUILayout.Button("Update", GUILayout.MaxWidth(50)) )
				{
					Undo.RecordObject(target, "Update");
					mod.UpdateWarp(i);
				}

				if ( GUILayout.Button("Delete", GUILayout.MaxWidth(50)) )
				{
					Undo.RecordObject(target, "Delete");
					mod.warps.RemoveAt(i);
				}

				EditorGUILayout.EndHorizontal();
			}

			if ( GUI.changed )
			{
				mod.Rebuild();
				EditorUtility.SetDirty(target);
			}
		}

		public void OnSceneGUI()
		{
			MegaBezPatch mod = (MegaBezPatch)target;

			if ( mod.showgizmos )
			{
				Handles.matrix = mod.transform.localToWorldMatrix;

				Handles.color = mod.latticecol;

				Handles.DrawLine(mod.p11, mod.p12);
				Handles.DrawLine(mod.p12, mod.p13);
				Handles.DrawLine(mod.p13, mod.p14);

				if ( !mod.align )
				{
					Handles.DrawLine(mod.p21, mod.p22);
					Handles.DrawLine(mod.p22, mod.p23);
					Handles.DrawLine(mod.p23, mod.p24);

					Handles.DrawLine(mod.p31, mod.p32);
					Handles.DrawLine(mod.p32, mod.p33);
					Handles.DrawLine(mod.p33, mod.p34);
				}

				Handles.DrawLine(mod.p41, mod.p42);
				Handles.DrawLine(mod.p42, mod.p43);
				Handles.DrawLine(mod.p43, mod.p44);

				Handles.DrawLine(mod.p11, mod.p21);
				Handles.DrawLine(mod.p21, mod.p31);
				Handles.DrawLine(mod.p31, mod.p41);

				if ( !mod.align )
				{
					Handles.DrawLine(mod.p12, mod.p22);
					Handles.DrawLine(mod.p22, mod.p32);
					Handles.DrawLine(mod.p32, mod.p42);

					Handles.DrawLine(mod.p13, mod.p23);
					Handles.DrawLine(mod.p23, mod.p33);
					Handles.DrawLine(mod.p33, mod.p43);
				}

				Handles.DrawLine(mod.p14, mod.p24);
				Handles.DrawLine(mod.p24, mod.p34);
				Handles.DrawLine(mod.p34, mod.p44);


				Quaternion rot = Quaternion.identity;
				if ( mod.showlabels )
				{
					Handles.Label(mod.p11, "11 " + mod.p11.ToString("0.00"));
					Handles.Label(mod.p14, "14 " + mod.p14.ToString("0.00"));
					Handles.Label(mod.p41, "41 " + mod.p41.ToString("0.00"));
					Handles.Label(mod.p44, "44 " + mod.p44.ToString("0.00"));

					if ( !mod.align )
					{
						Handles.Label(mod.p12, "12 " + mod.p12.ToString("0.00"));
						Handles.Label(mod.p13, "13 " + mod.p13.ToString("0.00"));

						Handles.Label(mod.p21, "21 " + mod.p21.ToString("0.00"));
						Handles.Label(mod.p22, "22 " + mod.p22.ToString("0.00"));
						Handles.Label(mod.p23, "23 " + mod.p23.ToString("0.00"));
						Handles.Label(mod.p24, "24 " + mod.p24.ToString("0.00"));

						Handles.Label(mod.p31, "31 " + mod.p31.ToString("0.00"));
						Handles.Label(mod.p32, "32 " + mod.p32.ToString("0.00"));
						Handles.Label(mod.p33, "33 " + mod.p33.ToString("0.00"));
						Handles.Label(mod.p34, "34 " + mod.p34.ToString("0.00"));

						Handles.Label(mod.p42, "42 " + mod.p42.ToString("0.00"));
						Handles.Label(mod.p43, "43 " + mod.p43.ToString("0.00"));
					}
				}

				Vector3 p11 = mod.p11;
				Vector3 p12 = mod.p12;
				Vector3 p13 = mod.p13;
				Vector3 p14 = mod.p14;

				Vector3 p21 = mod.p21;
				Vector3 p22 = mod.p22;
				Vector3 p23 = mod.p23;
				Vector3 p24 = mod.p24;

				Vector3 p31 = mod.p31;
				Vector3 p32 = mod.p32;
				Vector3 p33 = mod.p33;
				Vector3 p34 = mod.p34;

				Vector3 p41 = mod.p41;
				Vector3 p42 = mod.p42;
				Vector3 p43 = mod.p43;
				Vector3 p44 = mod.p44;

				if ( mod.positionhandles )
				{
					mod.p11 = MegaEditorGUILayout.PositionHandle(target, p11, rot);
					mod.p14 = MegaEditorGUILayout.PositionHandle(target, p14, rot);
					mod.p41 = MegaEditorGUILayout.PositionHandle(target, p41, rot);
					mod.p44 = MegaEditorGUILayout.PositionHandle(target, p44, rot);

					if ( !mod.align )
					{
						mod.p12 = MegaEditorGUILayout.PositionHandle(target, p12, rot);
						mod.p13 = MegaEditorGUILayout.PositionHandle(target, p13, rot);

						mod.p21 = MegaEditorGUILayout.PositionHandle(target, p21, rot);
						mod.p22 = MegaEditorGUILayout.PositionHandle(target, p22, rot);
						mod.p23 = MegaEditorGUILayout.PositionHandle(target, p23, rot);
						mod.p24 = MegaEditorGUILayout.PositionHandle(target, p24, rot);

						mod.p31 = MegaEditorGUILayout.PositionHandle(target, p31, rot);
						mod.p32 = MegaEditorGUILayout.PositionHandle(target, p32, rot);
						mod.p33 = MegaEditorGUILayout.PositionHandle(target, p33, rot);
						mod.p34 = MegaEditorGUILayout.PositionHandle(target, p34, rot);

						mod.p42 = MegaEditorGUILayout.PositionHandle(target, p42, rot);
						mod.p43 = MegaEditorGUILayout.PositionHandle(target, p43, rot);
					}
				}
				else
				{
					Handles.color = Color.green;

					mod.p11 = MegaEditorGUILayout.FreeHandle(target, p11, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
					mod.p14 = MegaEditorGUILayout.FreeHandle(target, p14, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
					mod.p41 = MegaEditorGUILayout.FreeHandle(target, p41, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
					mod.p44 = MegaEditorGUILayout.FreeHandle(target, p44, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);

					if ( !mod.align )
					{
						mod.p12 = MegaEditorGUILayout.FreeHandle(target, p12, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p13 = MegaEditorGUILayout.FreeHandle(target, p13, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);

						mod.p21 = MegaEditorGUILayout.FreeHandle(target, p21, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p22 = MegaEditorGUILayout.FreeHandle(target, p22, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p23 = MegaEditorGUILayout.FreeHandle(target, p23, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p24 = MegaEditorGUILayout.FreeHandle(target, p24, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);

						mod.p31 = MegaEditorGUILayout.FreeHandle(target, p31, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p32 = MegaEditorGUILayout.FreeHandle(target, p32, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p33 = MegaEditorGUILayout.FreeHandle(target, p33, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p34 = MegaEditorGUILayout.FreeHandle(target, p34, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);

						mod.p42 = MegaEditorGUILayout.FreeHandle(target, p42, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
						mod.p43 = MegaEditorGUILayout.FreeHandle(target, p43, rot, mod.handlesize, mod.snap, Handles.SphereHandleCap);
					}
				}

				bool dirty = false;
				if ( p11 != mod.p11 ) dirty = true;
				if ( p12 != mod.p12 ) dirty = true;
				if ( p13 != mod.p13 ) dirty = true;
				if ( p14 != mod.p14 ) dirty = true;

				if ( p21 != mod.p21 ) dirty = true;
				if ( p22 != mod.p22 ) dirty = true;
				if ( p23 != mod.p23 ) dirty = true;
				if ( p24 != mod.p24 ) dirty = true;

				if ( p31 != mod.p31 ) dirty = true;
				if ( p32 != mod.p32 ) dirty = true;
				if ( p33 != mod.p33 ) dirty = true;
				if ( p34 != mod.p34 ) dirty = true;

				if ( p41 != mod.p41 ) dirty = true;
				if ( p42 != mod.p42 ) dirty = true;
				if ( p43 != mod.p43 ) dirty = true;
				if ( p44 != mod.p44 ) dirty = true;

				if ( dirty )
					EditorUtility.SetDirty(target);
			}

			mod.p11.z = 0.0f;
			mod.p12.z = 0.0f;
			mod.p13.z = 0.0f;
			mod.p14.z = 0.0f;

			mod.p21.z = 0.0f;
			mod.p22.z = 0.0f;
			mod.p23.z = 0.0f;
			mod.p24.z = 0.0f;

			mod.p31.z = 0.0f;
			mod.p32.z = 0.0f;
			mod.p33.z = 0.0f;
			mod.p34.z = 0.0f;

			mod.p41.z = 0.0f;
			mod.p42.z = 0.0f;
			mod.p43.z = 0.0f;
			mod.p44.z = 0.0f;
		}
	}
}