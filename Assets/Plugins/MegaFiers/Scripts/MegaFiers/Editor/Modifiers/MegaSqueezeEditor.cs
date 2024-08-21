using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaSqueeze))]
	public class MegaSqueezeEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Squeeze Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaSqueeze mod = (MegaSqueeze)target;

			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Float(target, "Crv", ref mod.crv);
			MegaEditorGUILayout.Float(target, "Radial Amount", ref mod.radialamount);
			MegaEditorGUILayout.Float(target, "Radial Crv", ref mod.radialcrv);
			MegaEditorGUILayout.BeginToggle(target, "Do Region", ref mod.doRegion);
			MegaEditorGUILayout.Float(target, "From", ref mod.from);
			MegaEditorGUILayout.Float(target, "To", ref mod.to);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}