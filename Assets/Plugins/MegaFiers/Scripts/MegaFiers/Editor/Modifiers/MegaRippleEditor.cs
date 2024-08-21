using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaRipple))]
	public class MegaRippleEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Ripple Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaRipple mod = (MegaRipple)target;

			MegaEditorGUILayout.Float(target, "Amp", ref mod.amp);
			MegaEditorGUILayout.Float(target, "Amp 2", ref mod.amp2);
			MegaEditorGUILayout.Float(target, "Flex", ref mod.flex);
			MegaEditorGUILayout.Float(target, "Wave", ref mod.wave);
			MegaEditorGUILayout.Float(target, "Phase", ref mod.phase);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.Decay);
			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.Speed);
			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Int(target, "Segments", ref mod.segments);
			MegaEditorGUILayout.Int(target, "Circles", ref mod.circles);

			return false;
		}
	}
}