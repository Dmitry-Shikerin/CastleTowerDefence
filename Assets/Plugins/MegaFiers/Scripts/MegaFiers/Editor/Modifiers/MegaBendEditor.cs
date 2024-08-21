using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaBend))]
	public class MegaBendEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Bend Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaBend mod = (MegaBend)target;

			MegaEditorGUILayout.Float(target, "Angle", ref mod.angle);
			MegaEditorGUILayout.Toggle(target, "Use Radius", ref mod.useRadius);
			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Float(target, "Dir", ref mod.dir);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Toggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			return false;
		}
	}
}