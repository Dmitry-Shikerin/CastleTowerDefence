using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSkew))]
	public class MegaSkewEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Skew Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaSkew mod = (MegaSkew)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Dir", ref mod.dir);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}