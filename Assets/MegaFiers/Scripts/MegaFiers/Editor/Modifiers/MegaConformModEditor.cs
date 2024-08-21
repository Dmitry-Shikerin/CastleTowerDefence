using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaConformMod))]
	public class MegaConformModEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Conform Modifier by Chris West"; }

		public override bool DisplayCommon()
		{
			return false;
		}

		public override bool Inspector()
		{
			MegaConformMod mod = (MegaConformMod)target;

			CommonModParamsBasic(mod);

			MegaEditorGUILayout.GameObject(target, "Target", ref mod.target, true);
			MegaEditorGUILayout.Slider(target, "Conform Amount", ref mod.conformAmount, 0.0f, 1.0f);
			MegaEditorGUILayout.Float(target, "Ray Start Off", ref mod.raystartoff);
			MegaEditorGUILayout.Float(target, "Ray Dist", ref mod.raydist);
			MegaEditorGUILayout.Float(target, "Offset", ref mod.offset);
			MegaAxis axis = mod.axis;
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);

			if ( axis != mod.axis )
				mod.ChangeAxis();

			MegaEditorGUILayout.BeginToggle(target, "Use Local Down", ref mod.useLocalDown);
			MegaEditorGUILayout.Toggle(target, "Flip Down", ref mod.flipDown);
			MegaEditorGUILayout.Axis(target, "Down Axis", ref mod.downAxis);
			EditorGUILayout.EndToggleGroup();
			return false;
		}
	}
}