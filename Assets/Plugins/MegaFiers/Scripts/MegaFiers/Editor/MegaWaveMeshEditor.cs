using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWaveMesh))]
	public class MegaWaveMeshEditor : Editor
	{
		static bool showwave1 = true;
		static bool showwave2 = true;
		static bool showwave3 = true;

		[MenuItem("GameObject/Create Other/MegaShape/Wave Mesh")]
		static void CreateWaveMesh()
		{
			Vector3 pos = Vector3.zero;
			if ( UnityEditor.SceneView.lastActiveSceneView != null )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject("Wave Mesh");

			MeshFilter mf = go.AddComponent<MeshFilter>();
			mf.sharedMesh = new Mesh();
			MeshRenderer mr = go.AddComponent<MeshRenderer>();

			Material[] mats = new Material[1];

			mr.sharedMaterials = mats;
			MegaWaveMesh pm = go.AddComponent<MegaWaveMesh>();
			pm.mesh = mf.sharedMesh;
			go.transform.position = pos;
			Selection.activeObject = go;
			pm.Rebuild();
		}

		public override void OnInspectorGUI()
		{
			MegaWaveMesh mod = (MegaWaveMesh)target;

			MegaEditorGUILayout.Float(target, "Width", ref mod.Width);
			MegaEditorGUILayout.Float(target, "Length", ref mod.Length);
			MegaEditorGUILayout.Float(target, "Height", ref mod.Height);
			MegaEditorGUILayout.Int(target, "Width Segs", ref mod.WidthSegs);

			MegaEditorGUILayout.Toggle(target, "Link Scroll", ref mod.linkOffset);
			MegaEditorGUILayout.Float(target, "Offset", ref mod.offset);
			MegaEditorGUILayout.Toggle(target, "Recalc Bounds", ref mod.recalcBounds);
			MegaEditorGUILayout.Toggle(target, "Recalc Normals", ref mod.recalcNormals);

			MegaEditorGUILayout.BeginToggle(target, "Recalc Collider", ref mod.recalcCollider);
			MegaEditorGUILayout.Float(target, "Collider Width", ref mod.colwidth);
			MegaEditorGUILayout.Toggle(target, "Recalc Bounds", ref mod.smooth);
			MegaEditorGUILayout.EndToggle();

			MegaEditorGUILayout.Float(target, "Overall Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Overall Speed", ref mod.mspeed);

			MegaEditorGUILayout.BeginToggle(target, "Gen UVs", ref mod.GenUVs);
			MegaEditorGUILayout.Vector2(target, "UV Offset", ref mod.UVOffset);
			MegaEditorGUILayout.Vector2(target, "UV Scale", ref mod.UVScale);
			MegaEditorGUILayout.EndToggle();

			EditorGUILayout.BeginVertical();
			showwave1 = EditorGUILayout.Foldout(showwave1, "Wave 1");

			if ( showwave1 )
			{
				MegaEditorGUILayout.Float(target, "Flex", ref mod.flex);
				MegaEditorGUILayout.Float(target, "Amplitude", ref mod.amp);
				MegaEditorGUILayout.Float(target, "Wave Len", ref mod.wave);
				MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
				MegaEditorGUILayout.Float(target, "Time", ref mod.mtime);
				MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.Separator();

			EditorGUILayout.BeginVertical();
			showwave2 = EditorGUILayout.Foldout(showwave2, "Wave 2");

			if ( showwave2 )
			{
				MegaEditorGUILayout.Float(target, "Flex", ref mod.flex1);
				MegaEditorGUILayout.Float(target, "Amplitude", ref mod.amp1);
				MegaEditorGUILayout.Float(target, "Wave Len", ref mod.wave1);
				MegaEditorGUILayout.Float(target, "Phase", ref mod.phase1);
				MegaEditorGUILayout.Float(target, "Time", ref mod.mtime1);
				MegaEditorGUILayout.Float(target, "Speed", ref mod.speed1);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.Separator();

			EditorGUILayout.BeginVertical();

			showwave3 = EditorGUILayout.Foldout(showwave3, "Wave 3");

			if ( showwave3 )
			{
				MegaEditorGUILayout.Float(target, "Flex", ref mod.flex2);
				MegaEditorGUILayout.Float(target, "Amplitude", ref mod.amp2);
				MegaEditorGUILayout.Float(target, "Wave Len", ref mod.wave2);
				MegaEditorGUILayout.Float(target, "Phase", ref mod.phase2);
				MegaEditorGUILayout.Float(target, "Time", ref mod.mtime2);
				MegaEditorGUILayout.Float(target, "Speed", ref mod.speed2);
			}
			EditorGUILayout.EndVertical();

			if ( GUI.changed )
				mod.Rebuild();
		}
	}
}