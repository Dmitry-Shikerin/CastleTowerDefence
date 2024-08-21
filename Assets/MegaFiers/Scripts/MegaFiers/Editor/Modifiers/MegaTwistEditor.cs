using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaTwist))]
	public class MegaTwistEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Twist Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaTwist mod = (MegaTwist)target;

			MegaEditorGUILayout.Float(target, "Angle", ref mod.angle);
			MegaEditorGUILayout.Float(target, "Bias", ref mod.Bias);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();

			return false;
		}
	}
}