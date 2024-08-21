using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaScale))]
	public class MegaScaleEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Scale Modifier by Chris West"; }

		public override void OnInspectorGUI()
		{
			MegaScale mod = (MegaScale)target;

			mod.showModParams = EditorGUILayout.Foldout(mod.showModParams, "Modifier Common Params");

			if ( mod.showModParams )
				CommonModParamsBasic(mod);

			MegaEditorGUILayout.Vector3(target, "Scale", ref mod.scale);

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}
	}
}