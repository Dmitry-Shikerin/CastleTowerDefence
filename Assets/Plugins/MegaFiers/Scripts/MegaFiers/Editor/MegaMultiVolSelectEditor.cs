using UnityEngine;
using UnityEditor;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaMultiVolSelect))]
	public class MegaMultiVolSelectEditor : MegaModifierEditor
	{
		public override string GetHelpString() { return "Multi Vol Select Modifier by Chris West"; }
		public override bool DisplayCommon() { return false; }

		public override bool Inspector()
		{
			MegaMultiVolSelect mod = (MegaMultiVolSelect)target;

			MegaEditorGUILayout.Text(target, "Label", ref mod.Label);
			MegaEditorGUILayout.Int(target, "MaxLOD", ref mod.MaxLOD);
			MegaEditorGUILayout.Toggle(target, "Enabled", ref mod.ModEnabled);
			//int order = mod.Order;
			//MegaEditorGUILayout.Int(target, "Order", ref mod.Order);
			//if ( order != mod.Order )
			//{
				//MegaModifiers context = mod.GetComponent<MegaModifiers>();

				//if ( context != null )
					//context.BuildList();
			//}

			MegaEditorGUILayout.Toggle(target, "Freeze Selection", ref mod.freezeSelection);
			MegaEditorGUILayout.Toggle(target, "Use Stack Verts", ref mod.useCurrentVerts);
			MegaEditorGUILayout.Toggle(target, "Show Weights", ref mod.displayWeights);
			MegaEditorGUILayout.Color(target, "Gizmo Col", ref mod.gizCol);
			MegaEditorGUILayout.Float(target, "Gizmo Size", ref mod.gizSize);

			if ( GUILayout.Button("Add Volume") )
			{
				Undo.RecordObject(target, "Add Volume");
				mod.volumes.Add(MegaVolume.Create());
				EditorUtility.SetDirty(target);
			}

			for ( int v = 0; v < mod.volumes.Count; v++ )
			{
				MegaVolume vol = mod.volumes[v];

				MegaEditorGUILayout.BeginToggle(target, "Enabled", ref vol.enabled);
				MegaEditorGUILayout.VolType(target, "Type", ref vol.volType);

				if ( vol.volType == MegaVolumeType.Sphere )
					MegaEditorGUILayout.Float(target, "Radius", ref vol.radius);
				else
					MegaEditorGUILayout.Vector3(target, "Size", ref vol.boxsize);

				MegaEditorGUILayout.Slider(target, "Weight", ref vol.weight, 0.0f, 1.0f);
				MegaEditorGUILayout.Float(target, "Falloff", ref vol.falloff);
				MegaEditorGUILayout.Vector3(target, "Origin", ref vol.origin);
				MegaEditorGUILayout.Transform(target, "Target", ref vol.target, true);
				MegaEditorGUILayout.Toggle(target, "Inverse", ref vol.inverse);

				MegaEditorGUILayout.EndToggle();

				if ( GUILayout.Button("Delete Volume") )
				{
					Undo.RecordObject(target, "Delete Volume");
					mod.volumes.RemoveAt(v);
					v--;
					EditorUtility.SetDirty(target);
				}
			}

			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaMultiVolSelect mod = (MegaMultiVolSelect)target;

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

				Vector3 origin = Vector3.zero;

				for ( int v = 0; v < mod.volumes.Count; v++ )
				{
					MegaVolume vol = mod.volumes[v];

					if ( vol.enabled )
					{
						Handles.color = mod.gizCol;

						if ( vol.volType == MegaVolumeType.Sphere )
							MegaHandles.SphereCap(0, vol.origin, Quaternion.identity, vol.radius * 2.0f);

						if ( vol.target == null )
						{
							origin = MegaEditorGUILayout.PositionHandle(target, vol.origin, Quaternion.identity);

							if ( origin != vol.origin )
							{
								vol.origin = origin;
								EditorUtility.SetDirty(target);
							}
						}
					}
				}

				Handles.matrix = Matrix4x4.identity;
			}
		}
	}
}
#endif