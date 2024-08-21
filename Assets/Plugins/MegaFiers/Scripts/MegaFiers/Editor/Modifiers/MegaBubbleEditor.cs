using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaBubble))]
	public class MegaBubbleEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Bubble Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaBubble mod = (MegaBubble)target;

			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Float(target, "Falloff", ref mod.falloff);
			return false;
		}
	}
}