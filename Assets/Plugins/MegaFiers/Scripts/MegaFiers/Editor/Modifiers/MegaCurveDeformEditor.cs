using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaCurveDeform))]
	public class MegaCurveDeformEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Mega Curve Deform Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaCurveDeform mod = (MegaCurveDeform)target;

			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Curve(target, "Curve", ref mod.defCurve);
			MegaEditorGUILayout.Float(target, "Max Deviation", ref mod.MaxDeviation);
			MegaEditorGUILayout.BeginToggle(target, "Use Pos", ref mod.UsePos);
			MegaEditorGUILayout.Float(target, "Pos", ref mod.Pos);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}