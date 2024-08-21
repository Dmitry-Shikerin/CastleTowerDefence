using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaHump))]
	public class MegaHumpEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Hump Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaHump mod = (MegaHump)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Cycles", ref mod.cycles);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			return false;
		}
	}
}