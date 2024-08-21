using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSpherify))]
	public class MegaSpherifyEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Spherify Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaSpherify mod = (MegaSpherify)target;

			MegaEditorGUILayout.Float(target, "Percent", ref mod.percent);
			MegaEditorGUILayout.Float(target, "FallOff", ref mod.FallOff);
			return false;
		}
	}
}