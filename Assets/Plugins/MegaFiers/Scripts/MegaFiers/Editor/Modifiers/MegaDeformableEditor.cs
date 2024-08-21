using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaDeformable))]
	public class MegaDeformableEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Deformable Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaDeformable mod = (MegaDeformable)target;

			MegaEditorGUILayout.Float(target, "Hardness", ref mod.Hardness);
			Texture2D hmap = mod.HardnessMap;
			MegaEditorGUILayout.Texture2D(target, "Hardness Map", ref mod.HardnessMap);

			if ( hmap != mod.HardnessMap )
				mod.LoadMap();

			MegaEditorGUILayout.Float(target, "Impact Factor", ref mod.impactFactor);
			MegaEditorGUILayout.Float(target, "Collision Force", ref mod.ColForce);
			MegaEditorGUILayout.Float(target, "Max Vertex Move", ref mod.MaxVertexMov);
			MegaEditorGUILayout.Color32(target, "Deformed Color", ref mod.DeformedVertexColor);
			MegaEditorGUILayout.BeginToggle(target, "Use Decay", ref mod.usedecay);
			MegaEditorGUILayout.Float(target, "Decay", ref mod.decay);
			MegaEditorGUILayout.EndToggle();

			return false;
		}
	}
}