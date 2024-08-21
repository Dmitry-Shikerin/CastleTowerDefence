using UnityEditor;
using UnityEngine;

#if false
namespace MegaFiers
{
	[CustomEditor(typeof(MegaUVAdjust))]
	public class MegaUVAdjustEditor : MegaModifierEditor
	{
		public override bool Inspector()
		{
			MegaUVAdjust mod = (MegaUVAdjust)target;

			MegaEditorGUILayout.Toggle(target, "Animate", ref mod.animate);
			MegaEditorGUILayout.Float(target, "Rot Speed", ref mod.rotspeed);
			MegaEditorGUILayout.Float(target, "Spiral Speed", ref mod.spiralspeed);
			MegaEditorGUILayout.Vector3(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.Float(target, "Spiral", ref mod.spiral);
			MegaEditorGUILayout.Float(target, "Spiral Lim", ref mod.spirallim);
			return false;
		}

		public override void DrawSceneGUI()
		{
			MegaModifier mod = (MegaModifier)target;

			if ( mod.ModEnabled && mod.DisplayGizmo && MegaModifyObject.GlobalDisplay )
			{
				MegaModifyObject context = mod.GetComponent<MegaModifyObject>();

				if ( context != null && context.Enabled && context.DrawGizmos )
				{
					float a = mod.gizCol1.a;
					Color col = Color.white;

					Quaternion rot = mod.transform.localRotation;

					Handles.matrix = Matrix4x4.identity;

					if ( mod.Offset != Vector3.zero )
					{
						Vector3 pos = mod.transform.localToWorldMatrix.MultiplyPoint(Vector3.Scale(-mod.gizmoPos - mod.Offset, mod.bbox.Size()));
						Handles.Label(pos, mod.ModName() + " Pivot\n" + mod.Offset.ToString("0.000"));
						col = Color.blue;
						col.a = a;
						Handles.color = col;
						MegaHandles.ArrowCap(0, pos, rot * Quaternion.Euler(180.0f, 0.0f, 0.0f), mod.GizmoSize());
						col = Color.green;
						col.a = a;
						Handles.color = col;
						MegaHandles.ArrowCap(0, pos, rot * Quaternion.Euler(90.0f, 0.0f, 0.0f), mod.GizmoSize());
						col = Color.red;
						col.a = a;
						Handles.color = col;
						MegaHandles.ArrowCap(0, pos, rot * Quaternion.Euler(0.0f, -90.0f, 0.0f), mod.GizmoSize());
					}

					Handles.matrix = Matrix4x4.identity;
				}
			}
		}
	}
}
#endif