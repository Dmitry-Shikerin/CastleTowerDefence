using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaBendWarp))]
	public class MegaBendWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Bend")]
		static void CreateBendWarp() { CreateWarp("Bend", typeof(MegaBendWarp)); }

		public override string GetHelpString() { return "Bend Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaBendWarp mod = (MegaBendWarp)target;

			MegaEditorGUILayout.Float(target, "Angle", ref mod.angle);
			MegaEditorGUILayout.Toggle(target, "Use Radius", ref mod.useRadius);
			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
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