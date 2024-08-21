using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaNoise))]
	public class MegaNoiseEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Noise Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaNoise mod = (MegaNoise)target;

			MegaEditorGUILayout.Float(target, "Scale", ref mod.Scale);
			MegaEditorGUILayout.Float(target, "Freq", ref mod.Freq);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.Phase);
			MegaEditorGUILayout.Vector3(target, "Strength", ref mod.Strength);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.Animate);

			return false;
		}
	}
}