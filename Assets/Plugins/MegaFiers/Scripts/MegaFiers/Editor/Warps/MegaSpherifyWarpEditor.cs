using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSpherifyWarp))]
	public class MegaSpherifyWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Spherify")]
		static void CreateSpherifyWarp() { CreateWarp("Spherify", typeof(MegaSpherifyWarp)); }

		public override string GetHelpString() { return "Spherify Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaSpherifyWarp mod = (MegaSpherifyWarp)target;

			MegaEditorGUILayout.Float(target, "Percent", ref mod.percent);
			MegaEditorGUILayout.Float(target, "FallOff", ref mod.FallOff);
			return false;
		}
	}
}