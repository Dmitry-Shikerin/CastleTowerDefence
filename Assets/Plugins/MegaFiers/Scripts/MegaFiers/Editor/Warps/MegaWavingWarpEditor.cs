using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWavingWarp))]
	public class MegaWavingWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Waving")]
		static void CreateWavingWarp() { CreateWarp("Waving", typeof(MegaWavingWarp)); }

		public override string GetHelpString() { return "Waving Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaWavingWarp mod = (MegaWavingWarp)target;

			MegaEditorGUILayout.Float(target, "Amp", ref mod.amp);
			MegaEditorGUILayout.Float(target, "Wave", ref mod.wave);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.Speed);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.waveaxis);
			return false;
		}
	}
}