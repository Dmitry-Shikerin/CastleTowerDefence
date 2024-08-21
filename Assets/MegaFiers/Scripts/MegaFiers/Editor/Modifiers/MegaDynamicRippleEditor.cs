using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaDynamicRipple))]
	public class MegaDynamicRippleEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Dynamic Ripple Modifier by Chris West"; }

		public override bool Inspector()
		{
			MegaDynamicRipple mod = (MegaDynamicRipple)target;

			bool dirty = false;
			MegaEditorGUILayout.Axis(target, "Axis", ref mod.axis);

			MegaEditorGUILayout.Int(target, "Columns", ref mod.cols);
			if ( mod.cols < 1 )
				mod.cols = 1;

			MegaEditorGUILayout.Int(target, "Rows", ref mod.rows);
			if ( mod.rows < 1 )
				mod.rows = 1;

			if ( GUI.changed )
				dirty = true;

			MegaEditorGUILayout.Slider(target, "Damping", ref mod.damping, 0.0f, 1.0f);
			MegaEditorGUILayout.Slider(target, "Input Damping", ref mod.inputdamp, 0.0f, 1.0f);
			MegaEditorGUILayout.Slider(target, "Scale", ref mod.scale, 0.0f, 4.0f);
			MegaEditorGUILayout.Slider(target, "Speed", ref mod.speed, 0.0f, 0.5f);
			MegaEditorGUILayout.Float(target, "Force", ref mod.Force);
			MegaEditorGUILayout.Float(target, "InputForce", ref mod.InputForce);

			MegaEditorGUILayout.BeginToggle(target, "Obstructions", ref mod.Obstructions);
			bool bilin = mod.bilinearSample;
			MegaEditorGUILayout.Toggle(target, "Bilinear Sample", ref mod.bilinearSample);
			if ( bilin != mod.bilinearSample )
				dirty = true;
			Texture2D obtex = mod.obTexture;
			MegaEditorGUILayout.Texture2D(target, "Obstructions", ref mod.obTexture, true);
			EditorGUILayout.EndToggleGroup();
			if ( obtex != mod.obTexture )
				dirty = true;

			MegaEditorGUILayout.Float(target, "Drops Per Sec", ref mod.DropsPerSec);
			if ( mod.DropsPerSec < 0.0f )
				mod.DropsPerSec = 0.0f;

			if ( dirty )
				mod.ResetGrid();

			if ( GUILayout.Button("Reset Physics") )
			{
				Undo.RecordObject(target, "Reset Physics");
				mod.ResetGrid();
			}

			return false;
		}
	}
}