using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaRadialSkew))]
	public class MegaRadialSkewEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Radial Skew Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaRadialSkew mod = (MegaRadialSkew)target;

			MegaEditorGUILayout.Float(target, "Angle", ref mod.angle);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Axis(target, "Effective Axis", ref mod.eaxis);
			MegaEditorGUILayout.Toggle(target, "Bi Axial", ref mod.biaxial);
			return false;
		}
	}
}