using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaTreeBend))]
	public class MegaTreeBendEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Tree Bend Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaTreeBend mod = (MegaTreeBend)target;

			MegaEditorGUILayout.Float(target, "Bend Scale", ref mod.fBendScale);
			MegaEditorGUILayout.Float(target, "Wind Dir", ref mod.WindDir);
			MegaEditorGUILayout.Float(target, "Wind Speed", ref mod.WindSpeed);
			return false;
		}
	}
}