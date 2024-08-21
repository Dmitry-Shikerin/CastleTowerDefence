using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSimpleMod))]
	public class MegaSimpleEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Simple Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaSimpleMod mod = (MegaSimpleMod)target;

			MegaEditorGUILayout.Vector3(target, "A3", ref mod.a3);
			return false;
		}
	}
}