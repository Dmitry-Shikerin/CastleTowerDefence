using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaMeshPage))]
	public class MegaMeshPageEditor : Editor
	{
		[MenuItem("GameObject/Create Other/MegaShape/Page Mesh")]
		static void CreatePageMesh()
		{
			Vector3 pos = Vector3.zero;
			if ( UnityEditor.SceneView.lastActiveSceneView != null )
				pos = UnityEditor.SceneView.lastActiveSceneView.pivot;

			GameObject go = new GameObject("Page Mesh");
		
			MeshFilter mf = go.AddComponent<MeshFilter>();
			mf.sharedMesh = new Mesh();
			MeshRenderer mr = go.AddComponent<MeshRenderer>();

			Material[] mats = new Material[3];

			mr.sharedMaterials = mats;
			MegaMeshPage pm = go.AddComponent<MegaMeshPage>();

			go.transform.position = pos;
			Selection.activeObject = go;
			pm.Rebuild();
		}

		public override void OnInspectorGUI()
		{
			MegaMeshPage mod = (MegaMeshPage)target;

			MegaEditorGUILayout.Float(target, "Width", ref mod.Width);
			MegaEditorGUILayout.Float(target, "Length", ref mod.Length);
			MegaEditorGUILayout.Float(target, "Height", ref mod.Height);
			MegaEditorGUILayout.Int(target, "Width Segs", ref mod.WidthSegs);
			MegaEditorGUILayout.Int(target, "Length Segs", ref mod.LengthSegs);
			MegaEditorGUILayout.Int(target, "Height Segs", ref mod.HeightSegs);
			MegaEditorGUILayout.Toggle(target, "Gen UVs", ref mod.genUVs);
			MegaEditorGUILayout.Float(target, "Rotate", ref mod.rotate);
			MegaEditorGUILayout.Toggle(target, "Pivot Base", ref mod.PivotBase);
			MegaEditorGUILayout.Toggle(target, "Pivot Edge", ref mod.PivotEdge);
			MegaEditorGUILayout.Toggle(target, "Tangents", ref mod.tangents);

			if ( GUI.changed )
				mod.Rebuild();
		}
	}
}