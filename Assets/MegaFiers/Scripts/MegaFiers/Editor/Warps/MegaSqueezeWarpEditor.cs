using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSqueezeWarp))]
	public class MegaSqueezeWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Squeeze")]
		static void CreateSqueezeWarp() { CreateWarp("Squeeze", typeof(MegaSqueezeWarp)); }

		public override string GetHelpString() { return "Squeeze Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaSqueezeWarp mod = (MegaSqueezeWarp)target;

			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Crv", ref mod.crv);
			MegaEditorGUILayout.Float(target, "Radial Amount", ref mod.radialamount);
			MegaEditorGUILayout.Float(target, "Radial Crv", ref mod.radialcrv);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}