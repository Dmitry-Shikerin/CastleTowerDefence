using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWaving))]
	public class MegaWavingEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Waving Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaWaving mod = (MegaWaving)target;

			float amp = mod.amp * 100.0f;
			MegaEditorGUILayout.Float(target, "Amp", ref amp);
			mod.amp = amp * 0.01f;

			MegaEditorGUILayout.Float(target, "Wave", ref mod.wave);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.BeginToggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.Speed);
			MegaEditorGUILayout.EndToggle();
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.waveaxis);
			return false;
		}
	}
}