using UnityEditor;

namespace MegaFiers
{

	[CustomEditor(typeof(MegaCylindrifyWarp))]
	public class MegaCylindrifyWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Cylindrify")]
		static void CreateCylindrifyWarp() { CreateWarp("Cylindrify", typeof(MegaCylindrifyWarp)); }

		public override bool Inspector()
		{
			MegaCylindrifyWarp mod = (MegaCylindrifyWarp)target;

			MegaEditorGUILayout.Float(target, "Percent", ref mod.Percent);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			return false;
		}
	}
}