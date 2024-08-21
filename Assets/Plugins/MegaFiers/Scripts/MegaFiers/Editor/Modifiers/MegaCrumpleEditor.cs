using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaCrumple))]
	public class MegaCrumpleEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Crumple Modifier by Unity"; }

		public override bool Inspector()
		{
			MegaCrumple mod = (MegaCrumple)target;

			MegaEditorGUILayout.Float(target, "Scale", ref mod.scale);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			return false;
		}
	}
}