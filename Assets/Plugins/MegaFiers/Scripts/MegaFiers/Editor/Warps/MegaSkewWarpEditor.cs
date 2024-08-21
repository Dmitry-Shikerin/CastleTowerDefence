using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSkewWarp))]
	public class MegaSkewWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Skew")]
		static void CreateSkewWarp() { CreateWarp("Skew", typeof(MegaSkewWarp)); }

		public override string GetHelpString() { return "Skew Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaSkewWarp mod = (MegaSkewWarp)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Dir", ref mod.dir);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}