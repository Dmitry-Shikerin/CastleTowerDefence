using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaWaveWarp))]
	public class MegaWaveWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Wave")]
		static void CreateWaveWarp() { CreateWarp("Wave", typeof(MegaWaveWarp)); }

		public override string GetHelpString() { return "Wave Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaWaveWarp mod = (MegaWaveWarp)target;

			MegaEditorGUILayout.Float(target, "Amp", ref mod.amp);
			MegaEditorGUILayout.Float(target, "Amp 2", ref mod.amp2);
			MegaEditorGUILayout.Float(target, "Wave", ref mod.wave);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.Speed);
			MegaEditorGUILayout.Int(target, "Divs", ref mod.divs);
			MegaEditorGUILayout.Int(target, "Num Segs", ref mod.numSegs);
			MegaEditorGUILayout.Int(target, "Num Sides", ref mod.numSides);
			return false;
		}
	}
}