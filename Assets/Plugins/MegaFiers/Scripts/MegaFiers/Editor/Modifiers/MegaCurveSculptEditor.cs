using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaCurveSculpt))]
	public class MegaCurveSculptEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Mega Curve Sculpt Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaCurveSculpt mod = (MegaCurveSculpt)target;

			MegaEditorGUILayout.Vector3(target, "Offset Amount", ref mod.OffsetAmount);
			MegaEditorGUILayout.Axis(target, "Alter", ref mod.offsetX);
			MegaEditorGUILayout.Curve(target, "Offset X", ref mod.defCurveX);
			MegaEditorGUILayout.Axis(target, "Alter", ref mod.offsetY);
			MegaEditorGUILayout.Curve(target, "Offset Y", ref mod.defCurveY);
			MegaEditorGUILayout.Axis(target, "Alter", ref mod.offsetX);
			MegaEditorGUILayout.Curve(target, "Offset Z", ref mod.defCurveZ);

			MegaEditorGUILayout.Vector3(target, "Scale Amount", ref mod.ScaleAmount);
			MegaEditorGUILayout.Axis(target, "Alter", ref mod.scaleX);
			MegaEditorGUILayout.Curve(target, "Scale X", ref mod.defCurveSclX);
			MegaEditorGUILayout.Axis(target, "Alter", ref mod.scaleY);
			MegaEditorGUILayout.Curve(target, "Scale Y", ref mod.defCurveSclY);
			MegaEditorGUILayout.Axis(target, "Alter", ref mod.scaleX);
			MegaEditorGUILayout.Curve(target, "Scale Z", ref mod.defCurveSclZ);

			return false;
		}
	}
}