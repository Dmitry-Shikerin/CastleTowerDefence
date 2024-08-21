using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaCylindrify))]
	public class MegaCylindrifyEditor : MegaModifierEditor
	{
		public override bool Inspector()
		{
			MegaCylindrify mod = (MegaCylindrify)target;

			MegaEditorGUILayout.Float(target, "Percent", ref mod.Percent);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			return false;
		}
	}
}