using UnityEngine;
using UnityEditor;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaVolSelect))]
	public class MegaVolSelectEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Vol Select Modifier by Chris West"; }

		public override bool DisplayCommon() { return false; }

		public override bool Inspector()
		{
			MegaVolSelect mod = (MegaVolSelect)target;

			MegaEditorGUILayout.Text(target, "Label", ref mod.Label);
			MegaEditorGUILayout.Int(target, "MaxLOD", ref mod.MaxLOD);
			MegaEditorGUILayout.Toggle(target, "Enabled", ref mod.ModEnabled);
			//MegaEditorGUILayout.Int(target, "Order", ref mod.Order);
			MegaEditorGUILayout.VolType(target, "Type", ref mod.volType);

			if ( mod.volType == MegaVolumeType.Sphere )
				MegaEditorGUILayout.Float(target, "Radius", ref mod.radius);
			else
				MegaEditorGUILayout.Vector3(target, "Size", ref mod.boxsize);

			MegaEditorGUILayout.Slider(target, "Weight", ref mod.weight, 0.0f, 1.0f);
			MegaEditorGUILayout.Float(target, "Falloff", ref mod.falloff);
			MegaEditorGUILayout.Vector3(target, "Origin", ref mod.origin);
			MegaEditorGUILayout.Transform(target, "Target", ref mod.target, true);
			MegaEditorGUILayout.Toggle(target, "Use Stack Verts", ref mod.useCurrentVerts);
			MegaEditorGUILayout.Toggle(target, "Show Weights", ref mod.displayWeights);
			MegaEditorGUILayout.Color(target, "Gizmo Col", ref mod.gizCol);
			MegaEditorGUILayout.Float(target, "Gizmo Size", ref mod.gizSize);
			MegaEditorGUILayout.Toggle(target, "Freeze Selection", ref mod.freezeSelection);
			MegaEditorGUILayout.Toggle(target, "Inverse", ref mod.inverse);

			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaVolSelect mod = (MegaVolSelect)target;

			if ( !mod.ModEnabled )
				return;

			MegaModifyObject mc = mod.gameObject.GetComponent<MegaModifyObject>();

			float[] sel = mod.GetSel();

			if ( mc != null && sel != null )
			{
				Matrix4x4 tm = mod.gameObject.transform.localToWorldMatrix;
				Handles.matrix = tm;

				if ( mod.displayWeights )
				{
					for ( int i = 0; i < sel.Length; i++ )
					{
						float w = sel[i];
						if ( w > 0.001f )
						{
							if ( w > 0.5f )
								Handles.color = Color.Lerp(Color.green, Color.red, (w - 0.5f) * 2.0f);
							else
								Handles.color = Color.Lerp(Color.blue, Color.green, w * 2.0f);

							MegaHandles.DotCap(i, mc.jsverts[i], Quaternion.identity, mod.gizSize);
						}
					}
				}

				Handles.color = mod.gizCol;

				if ( mod.volType == MegaVolumeType.Sphere )
					MegaHandles.SphereCap(0, tm.MultiplyPoint(mod.origin), Quaternion.identity, mod.radius * 2.0f);

				Handles.matrix = tm;
			
				Vector3 origin = mod.origin;
				mod.origin = MegaEditorGUILayout.PositionHandle(target, mod.origin, Quaternion.identity);

				if ( origin != mod.origin )
					EditorUtility.SetDirty(target);

				Handles.matrix = Matrix4x4.identity;
			}
		}
	}
}
#endif