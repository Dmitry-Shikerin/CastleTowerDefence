using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaGlobe))]
	public class MegaGlobeEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Globe Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaGlobe mod = (MegaGlobe)target;

			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Toggle(target, "Link Radii", ref mod.linkRadii);
			MegaEditorGUILayout.Float(target, "Radius1", ref mod.radius1);
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