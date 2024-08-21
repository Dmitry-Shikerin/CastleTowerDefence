using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaAttach))]
	public class MegaAttachEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaAttach mod = (MegaAttach)target;

			MegaEditorGUILayout.ModifierObject(target, "Target", ref mod.target, true);
			//MegaEditorGUILayout.Vector3(target, "Attach Fwd", ref mod.attachforward);
			MegaEditorGUILayout.Vector3(target, "Axis Rot", ref mod.AxisRot);
			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			//MegaEditorGUILayout.Vector3(target, "Up", ref mod.up);
			//MegaEditorGUILayout.Toggle(target, "World Space", ref mod.worldSpace);

			if ( GUI.changed )
				EditorUtility.SetDirty(mod);

			if ( !mod.attached )
			{
				if ( GUILayout.Button("Attach") )
				{
					Undo.RecordObject(target, "Attach");
					mod.AttachIt();
					EditorUtility.SetDirty(mod);
				}
			}
			else
			{
				if ( GUILayout.Button("Detach") )
				{
					Undo.RecordObject(target, "Detatch");
					mod.DetachIt();
					EditorUtility.SetDirty(mod);
				}
			}
		}
	}
}