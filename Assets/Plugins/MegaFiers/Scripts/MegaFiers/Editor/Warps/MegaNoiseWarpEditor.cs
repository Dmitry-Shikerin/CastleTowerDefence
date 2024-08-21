using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaNoiseWarp))]
	public class MegaNoiseWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Noise")]
		static void CreateStarShape() { CreateWarp("Noise", typeof(MegaNoiseWarp)); }

		public override string GetHelpString() { return "Noise Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaNoiseWarp mod = (MegaNoiseWarp)target;

			MegaEditorGUILayout.Float(target, "Scale", ref mod.Scale);
			MegaEditorGUILayout.Float(target, "Freq", ref mod.Freq);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.Phase);
			MegaEditorGUILayout.Vector3(target, "Strength", ref mod.Strength);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.Animate);
			return false;
		}
	}
}