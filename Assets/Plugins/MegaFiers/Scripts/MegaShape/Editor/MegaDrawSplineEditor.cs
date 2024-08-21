
using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaDrawSpline))]
	public class MegaDrawSplineEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaDrawSpline mod = (MegaDrawSpline)target;

			MegaEditorGUILayout.Slider(target, "Update Dist", ref mod.updatedist, 0.02f, 100.0f);
			MegaEditorGUILayout.Slider(target, "Smooth", ref mod.smooth, 0.0f, 1.5f);
			MegaEditorGUILayout.Float(target, "Offset", ref mod.offset);
			MegaEditorGUILayout.Float(target, "Gizmo Radius", ref mod.radius);
			MegaEditorGUILayout.Float(target, "Mesh Step", ref mod.meshstep);
			MegaEditorGUILayout.MeshShapeType(target, "Mesh Type", ref mod.meshtype);
			MegaEditorGUILayout.Float(target, "Width", ref mod.width);
			MegaEditorGUILayout.Float(target, "Height", ref mod.height);
			MegaEditorGUILayout.Float(target, "Tube Radius", ref mod.tradius);
			MegaEditorGUILayout.Material(target, "Material", ref mod.mat, true);
			MegaEditorGUILayout.Toggle(target, "Build Closed", ref mod.closed);
			MegaEditorGUILayout.Slider(target, "Close Value", ref mod.closevalue, 0.0f, 1.0f);
			MegaEditorGUILayout.Toggle(target, "Constant Speed", ref mod.constantspd);

			if ( GUI.changed )
				EditorUtility.SetDirty(mod);
		}
	}
}