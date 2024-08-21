using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaRippleWarp))]
	public class MegaRippleWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Ripple")]
		static void CreateRippleWarp() { CreateWarp("Ripple", typeof(MegaRippleWarp)); }

		public override string GetHelpString() { return "Ripple Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaRippleWarp mod = (MegaRippleWarp)target;

			MegaEditorGUILayout.Float(target, "Amp", ref mod.amp);
			MegaEditorGUILayout.Float(target, "Amp 2", ref mod.amp2);
			MegaEditorGUILayout.Float(target, "Flex", ref mod.flex);
			MegaEditorGUILayout.Float(target, "Wave", ref mod.wave);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.Speed);
			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Int(target, "Segments", ref mod.segments);
			MegaEditorGUILayout.Int(target, "Circles", ref mod.circles);
			return false;
		}
	}
}