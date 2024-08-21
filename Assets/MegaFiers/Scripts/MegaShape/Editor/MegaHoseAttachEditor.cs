using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaHoseAttach))]
	public class MegaHoseAttachEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaHoseAttach mod = (MegaHoseAttach)target;

			MegaEditorGUILayout.Hose(target, "Hose", ref mod.hose, true);
			MegaEditorGUILayout.Float(target, "Alpha", ref mod.alpha);
			MegaEditorGUILayout.Vector3(target, "Offset", ref mod.offset);
			MegaEditorGUILayout.BeginToggle(target, "Rot On", ref mod.rot);
			MegaEditorGUILayout.Vector3(target, "Rotate", ref mod.rotate);
			MegaEditorGUILayout.EndToggle();
			MegaEditorGUILayout.Toggle(target, "Late Update", ref mod.doLateUpdate);

			if ( GUI.changed )
				EditorUtility.SetDirty(mod);
		}
	}
}