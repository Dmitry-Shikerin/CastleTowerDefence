using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaStretch))]
	public class MegaStretchEditor : MegaModifierEditor
	{
		public override string GetHelpString()	{ return "Stretch Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaStretch mod = (MegaStretch)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Amplify", ref mod.amplify);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}