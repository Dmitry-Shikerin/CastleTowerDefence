using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaPaint))]
	public class MegaPaintEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Vertex Paint Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaPaint mod = (MegaPaint)target;

			MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			MegaEditorGUILayout.Float(target, "Amount", ref mod.amount);
			MegaEditorGUILayout.Toggle(target, "Use Decay", ref mod.usedecay);

			if ( mod.usedecay )
				MegaEditorGUILayout.Float(target, "Decay", ref mod.decay);

			MegaEditorGUILayout.FallOff(target, "Falloff Mode", ref mod.fallOff);
			MegaEditorGUILayout.Float(target, "Falloff", ref mod.gaussc);

			MegaEditorGUILayout.Toggle(target, "Use Avg Norm", ref mod.useAvgNorm);

			if ( !mod.useAvgNorm )
				MegaEditorGUILayout.Vector3(target, "Normal", ref mod.normal);

			MegaEditorGUILayout.PaintMode(target, "Paint Mode", ref mod.mode);

			return false;
		}
	}
}