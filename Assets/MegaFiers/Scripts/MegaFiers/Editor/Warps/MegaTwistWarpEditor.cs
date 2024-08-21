using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaTwistWarp))]
	public class MegaTwistWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Twist")]
		static void CreateTwistWarp() { CreateWarp("Twist", typeof(MegaTwistWarp)); }

		public override string GetHelpString() { return "Twist Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaTwistWarp mod = (MegaTwistWarp)target;

			MegaEditorGUILayout.Float(target, "Angle", ref mod.angle);
			MegaEditorGUILayout.Float(target, "Bias", ref mod.Bias);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}