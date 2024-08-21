using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaRubber))]
	public class MegaRubberEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Rubber Modifier by Chris West"; }
		public override bool DisplayCommon()	{ return false; }

		public override bool Inspector()
		{
			MegaRubber mod = (MegaRubber)target;

			MegaEditorGUILayout.Text(target, "Label", ref mod.Label);
			MegaEditorGUILayout.Int(target, "MaxLOD", ref mod.MaxLOD);
			MegaEditorGUILayout.Toggle(target, "Enabled", ref mod.ModEnabled);
			Transform trg = mod.target;
			MegaEditorGUILayout.Transform(target, "Target", ref mod.target, true);

			if ( trg != mod.target )
				mod.SetTarget(trg);

			MegaRubberType mattype = mod.Presets;
			MegaEditorGUILayout.RubberType(target, "Material", ref mod.Presets);

			if ( mattype != mod.Presets )
				mod.ChangeMaterial();

			MegaWeightChannel channel = mod.channel;
			MegaEditorGUILayout.WeightChannel(target, "Channel", ref mod.channel);

			if ( channel != mod.channel )
				mod.ChangeChannel();

			channel = mod.stiffchannel;
			MegaEditorGUILayout.WeightChannel(target, "Stiff Channel", ref mod.stiffchannel);

			if ( channel != mod.stiffchannel )
				mod.ChangeChannel();

			MegaEditorGUILayout.Slider(target, "Threshhold", ref mod.threshold, 0.0f, 1.0f);
			if ( GUILayout.Button("Apply Threshold") )
			{
				mod.ChangeChannel();
				EditorUtility.SetDirty(target);
			}

			MegaEditorGUILayout.Vector3(target, "Intensity", ref mod.Intensity);
			MegaEditorGUILayout.Float(target, "Gravity", ref mod.gravity);
			MegaEditorGUILayout.Vector3(target, "Damping", ref mod.damping);
			MegaEditorGUILayout.Float(target, "Mass", ref mod.mass);
			MegaEditorGUILayout.Vector3(target, "Stiffness", ref mod.stiffness);

			MegaEditorGUILayout.Toggle(target, "Show Weights", ref mod.showweights);
			float size = mod.size * 100.0f;
			MegaEditorGUILayout.Float(target, "Size", ref size);
			mod.size = size * 0.01f;
			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaRubber mod = (MegaRubber)target;
			if ( mod.showweights && mod.vr != null )
			{
				Color col = Color.black;

				Matrix4x4 tm = mod.gameObject.transform.localToWorldMatrix;
				Handles.matrix = tm;

				for ( int i = 0; i < mod.vr.Length; i++ )
				{
					float w = mod.vr[i].weight;
					if ( w > 0.5f )
						col = Color.Lerp(Color.green, Color.red, (w - 0.5f) * 2.0f);
					else
						col = Color.Lerp(Color.blue, Color.green, w * 2.0f);
					Handles.color = col;

					Vector3 p = mod.vr[i].cpos;
					MegaHandles.DotCap(i, p, Quaternion.identity, mod.size);
				}

				Handles.matrix = Matrix4x4.identity;
			}
		}
	}
}