using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSinusCurve))]
	public class MegaSinusCurveEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Sinus Curve Modifier by Unity"; }

		public override bool Inspector()
		{
			MegaSinusCurve mod = (MegaSinusCurve)target;

			MegaEditorGUILayout.Float(target, "Scale", ref mod.scale);
			MegaEditorGUILayout.Float(target, "Wave", ref mod.wave);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			return false;
		}
	}
}