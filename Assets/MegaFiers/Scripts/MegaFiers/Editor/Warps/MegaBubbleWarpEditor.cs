using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaBubbleWarp))]
	public class MegaBubbleWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Bubble")]
		static void CreateBubbleWarp() { CreateWarp("Bubble", typeof(MegaBubbleWarp)); }

		public override string GetHelpString() { return "Bubble Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaBubbleWarp mod = (MegaBubbleWarp)target;

			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Float(target, "Falloff", ref mod.falloff);
			return false;
		}
	}
}