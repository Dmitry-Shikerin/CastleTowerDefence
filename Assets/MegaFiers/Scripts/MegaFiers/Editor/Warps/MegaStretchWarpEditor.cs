using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaStretchWarp))]
	public class MegaStretchWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Stretch")]
		static void CreateStretchWarp() { CreateWarp("Stretch", typeof(MegaStretchWarp)); }

		public override string GetHelpString() { return "Stretch Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaStretchWarp mod = (MegaStretchWarp)target;

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