using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaTaperWarp))]
	public class MegaTaperWarpEditor : MegaWarpEditor
	{
		[MenuItem("GameObject/Create Other/MegaFiers/Warps/Taper")]
		static void CreateTaperWarp() { CreateWarp("Taper", typeof(MegaTaperWarp)); }

		public override string GetHelpString() { return "Taper Warp Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaTaperWarp mod = (MegaTaperWarp)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Crv", ref mod.crv);
			MegaEditorGUILayout.Float(target, "Dir", ref mod.dir);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.EffectAxis(target, "EAxis", ref mod.EAxis);
			MegaEditorGUILayout.Toggle(target, "Sym", ref mod.sym);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}