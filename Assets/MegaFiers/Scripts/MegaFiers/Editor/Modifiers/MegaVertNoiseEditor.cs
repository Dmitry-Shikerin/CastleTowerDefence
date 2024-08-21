using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaVertNoise))]
	public class MegaVertNoiseEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Vertical Noise Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaVertNoise mod = (MegaVertNoise)target;

			MegaEditorGUILayout.Float(target, "Scale", ref mod.Scale);
			MegaEditorGUILayout.Float(target, "Freq", ref mod.Freq);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.Phase);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.decay);
			MegaEditorGUILayout.Float(target, "Strength", ref mod.Strength);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.Animate);

			return false;
		}
	}
}