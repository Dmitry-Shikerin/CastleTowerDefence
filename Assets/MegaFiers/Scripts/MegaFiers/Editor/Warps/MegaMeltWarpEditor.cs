using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaMeltWarp))]
	public class MegaMeltWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Melt")]
		static void CreateMeltWarp() { CreateWarp("Melt", typeof(MegaMeltWarp)); }

		public override string GetHelpString() { return "Melt Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaMeltWarp mod = (MegaMeltWarp)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.Amount);
			MegaEditorGUILayout.Float(target, "Spread", ref mod.Spread);
			MegaEditorGUILayout.MeltMat(target, "Material Type", ref mod.MaterialType);
			MegaEditorGUILayout.Float(target, "Solidity", ref mod.Solidity);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Toggle(target, "Flip Axis", ref mod.FlipAxis);
			MegaEditorGUILayout.Slider(target, "Flatness", ref mod.flatness, 0.0f, 1.0f);

			return false;
		}
	}
}