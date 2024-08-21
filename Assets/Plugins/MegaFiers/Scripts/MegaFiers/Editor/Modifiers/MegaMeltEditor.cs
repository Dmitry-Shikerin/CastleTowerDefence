using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CanEditMultipleObjects, CustomEditor(typeof(MegaMelt))]
	public class MegaMeltEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Melt Modifier by Chris West"; }

		public override void Enable()
		{
		}

		public override bool Inspector()
		{
			MegaMelt mod = (MegaMelt)target;

			MegaEditorGUILayout.Float(target, "Amount", ref mod.Amount);
			MegaEditorGUILayout.Float(target, "Spread", ref mod.Spread);
			MegaEditorGUILayout.MeltMat(target, "Material Type", ref mod.MaterialType);
			MegaEditorGUILayout.Float(target, "Solidity", ref mod.Solidity);
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);
			MegaEditorGUILayout.Toggle(target, "Flip Axis", ref mod.FlipAxis);
			MegaEditorGUILayout.Slider(target, "Flatness", ref mod.flatness, 0.0f, 1.0f);
			return false;
		}
	}
}