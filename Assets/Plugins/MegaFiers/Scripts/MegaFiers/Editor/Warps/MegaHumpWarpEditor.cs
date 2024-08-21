using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaHumpWarp))]
	public class MegaHumpWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Hump")]
		static void CreateHumpWarp() { CreateWarp("Hump", typeof(MegaHumpWarp)); }

		public override string GetHelpString() { return "Hump Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaHumpWarp mod = (MegaHumpWarp)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Cycles", ref mod.cycles);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			return false;
		}
	}
}