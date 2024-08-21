using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPush))]
	public class MegaPushEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Push Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaPush mod = (MegaPush)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.NormType(target, "Method", ref mod.method);
			return false;
		}
	}
}