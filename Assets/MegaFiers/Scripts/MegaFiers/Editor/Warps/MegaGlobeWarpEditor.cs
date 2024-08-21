using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaGlobeWarp))]
	public class MegaGlobeWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Globe")]
		static void CreateStarShape() { CreateWarp("Globe", typeof(MegaGlobeWarp)); }

		public override string GetHelpString() { return "Globe Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaGlobeWarp mod = (MegaGlobeWarp)target;

			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.BeginToggle(target, "Link Radii", ref mod.linkRadii);
			MegaEditorGUILayout.Float(target, "Radius1", ref mod.radius1);
			EditorGUILayout.EndToggleGroup();
			MegaEditorGUILayout.Float(target, "Dir", ref mod.dir);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.BeginToggle(target, "Two Axis", ref mod.twoaxis);
			MegaEditorGUILayout.Float(target, "Dir1", ref mod.dir1);
			MegaEditorGUILayout.Axis(target, "Axis1", ref mod.axis1);
			EditorGUILayout.EndToggleGroup();

			return false;
		}
	}
}