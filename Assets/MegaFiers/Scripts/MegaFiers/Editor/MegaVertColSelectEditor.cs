using UnityEngine;
using UnityEditor;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaVertColSelect))]
	public class MegaVertColSelectEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Vertex Color Select Modifier by Chris West"; }
		public override bool DisplayCommon() { return false; }

		public override bool Inspector()
		{
			MegaVertColSelect mod = (MegaVertColSelect)target;

			MegaEditorGUILayout.Text(target, "Label", ref mod.Label);
			MegaEditorGUILayout.Int(target, "MaxLOD", ref mod.MaxLOD);
			MegaEditorGUILayout.Toggle(target, "Enabled", ref mod.ModEnabled);
			//MegaEditorGUILayout.Int(target, "Order", ref mod.Order);
			MegaEditorGUILayout.Float(target, "Weight", ref mod.weight);
			MegaEditorGUILayout.Slider(target, "Threshold", ref mod.threshold, 0.0f, 1.0f);
			MegaEditorGUILayout.Channel(target, "Channel", ref mod.channel);
			MegaEditorGUILayout.Toggle(target, "Show Weights", ref mod.displayWeights);
			MegaEditorGUILayout.Float(target, "Gizmo Size", ref mod.gizSize);

			if ( GUI.changed )
				mod.update = true;

			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaVertColSelect mod = (MegaVertColSelect)target;

			MegaModifyObject mc = mod.gameObject.GetComponent<MegaModifyObject>();

			float[] sel = mod.GetSel();

			if ( mc != null && sel != null )
			{
				Color col = Color.black;

				Matrix4x4 tm = mod.gameObject.transform.localToWorldMatrix;
				Handles.matrix = Matrix4x4.identity;

				if ( mod.displayWeights )
				{
					for ( int i = 0; i < sel.Length; i++ )
					{
						float w = sel[i];
						if ( w > 0.5f )
							col = Color.Lerp(Color.green, Color.red, (w - 0.5f) * 2.0f);
						else
							col = Color.Lerp(Color.blue, Color.green, w * 2.0f);
						Handles.color = col;

						Vector3 p = tm.MultiplyPoint(mc.jsverts[i]);

						if ( w > 0.001f )
							MegaHandles.DotCap(i, p, Quaternion.identity, mod.gizSize);
					}
				}
			}
		}
	}
}
#endif