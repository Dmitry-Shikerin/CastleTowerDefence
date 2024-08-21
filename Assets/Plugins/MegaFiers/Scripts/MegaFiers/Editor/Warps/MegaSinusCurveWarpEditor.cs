using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSinusCurveWarp))]
	public class MegaSinusCurveWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Sinus Curve")]
		static void CreateSinusWarp() { CreateWarp("Sinus", typeof(MegaSinusCurveWarp)); }

		public override string GetHelpString() { return "Sinus Curve Warp Modifier by Unity"; }

		public override bool Inspector()
		{
			MegaSinusCurveWarp mod = (MegaSinusCurveWarp)target;

			MegaEditorGUILayout.Float(target, "Scale", ref mod.scale);
			MegaEditorGUILayout.Float(target, "Wave", ref mod.wave);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			return false;
		}
	}
}