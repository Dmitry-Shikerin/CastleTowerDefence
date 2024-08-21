using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaZStretchWarp))]
	public class MegaZStretchWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/ZStretch")]
		static void CreateZStretchWarp() { CreateWarp("ZStretch", typeof(MegaZStretchWarp)); }

		public override string GetHelpString() { return "ZStretch Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaZStretchWarp mod = (MegaZStretchWarp)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Amplify", ref mod.amplify);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}