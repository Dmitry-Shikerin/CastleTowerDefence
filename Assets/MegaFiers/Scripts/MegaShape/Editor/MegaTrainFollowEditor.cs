using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaTrainFollow))]
	public class MegaTrainFollowEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			MegaTrainFollow mod = (MegaTrainFollow)target;

			MegaEditorGUILayout.ShapeObject(target, "Path", ref mod.path, true);

			if ( mod.path && mod.path.splines != null )
			{
				if ( mod.path.splines.Count > 1 )
					MegaEditorGUILayout.Int(target, "Curve", ref mod.curve, 0, mod.path.splines.Count - 1);

				if ( mod.curve < 0 )	mod.curve = 0;
				if ( mod.curve > mod.path.splines.Count - 1 )
					mod.curve = mod.path.splines.Count - 1;
			}

			MegaEditorGUILayout.Float(target, "Distance", ref mod.distance);
			MegaEditorGUILayout.Float(target, "Speed", ref mod.speed);
			MegaEditorGUILayout.Toggle(target, "Show Rays", ref mod.showrays);

			if ( mod.carriages.Count < 1 )
			{
				if ( GUILayout.Button("Add") )
				{
					Undo.RecordObject(target, "Add");
					MegaCarriage car = new MegaCarriage();
					mod.carriages.Add(car);
				}
			}

			for ( int i = 0; i < mod.carriages.Count; i++ )
			{
				MegaCarriage car = mod.carriages[i];

				EditorGUILayout.BeginVertical("Box");

				MegaEditorGUILayout.Float(target, "Length", ref car.length);
				MegaEditorGUILayout.Float(target, "Bogey Off", ref car.bogeyoff);
				MegaEditorGUILayout.GameObject(target, "Carriage", ref car.carriage, true);
				MegaEditorGUILayout.Vector3(target, "Carriage Off", ref car.carriageOffset);
				MegaEditorGUILayout.Vector3(target, "Carriage Rot", ref car.rot);
				MegaEditorGUILayout.GameObject(target, "Front Bogey", ref car.bogey1, true);
				MegaEditorGUILayout.Vector3(target, "Front Bogey Off", ref car.bogey1Offset);
				MegaEditorGUILayout.Vector3(target, "Front Bogey Rot", ref car.bogey1Rot);
				MegaEditorGUILayout.GameObject(target, "Rear Bogey", ref car.bogey2, true);
				MegaEditorGUILayout.Vector3(target, "Rear Bogey Off", ref car.bogey2Offset);
				MegaEditorGUILayout.Vector3(target, "Rear Bogey Rot", ref car.bogey2Rot);

				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginHorizontal();

				if ( GUILayout.Button("Add") )
				{
					Undo.RecordObject(target, "Add");
					MegaCarriage nc = new MegaCarriage();
					mod.carriages.Add(nc);
				}

				if ( GUILayout.Button("Delete") )
				{
					Undo.RecordObject(target, "Delete");
					mod.carriages.Remove(car);
				}

				EditorGUILayout.EndHorizontal();
			}

			if ( GUI.changed )
				EditorUtility.SetDirty(target);
		}

		[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable | GizmoType.InSelectionHierarchy)]
		static void RenderGizmo(MegaTrainFollow mod, GizmoType gizmoType)
		{
			if ( (gizmoType & GizmoType.Active) != 0 && Selection.activeObject == mod.gameObject )
			{
				if ( !mod.showrays )
					return;

				for ( int i = 0; i < mod.carriages.Count; i++ )
				{
					MegaCarriage car = mod.carriages[i];

					Handles.color = Color.white;
					Handles.DrawLine(car.b1, car.b2);
					Handles.SphereHandleCap(0, car.cp, Quaternion.identity, car.length * 0.025f, EventType.Layout);
					Handles.SphereHandleCap(0, car.b1, Quaternion.identity, car.length * 0.025f, EventType.Layout);
					Handles.SphereHandleCap(0, car.b2, Quaternion.identity, car.length * 0.025f, EventType.Layout);
					Handles.color = Color.red;
					Handles.DrawLine(car.cp, car.bp1);
					Handles.SphereHandleCap(0, car.bp1, Quaternion.identity, car.length * 0.025f, EventType.Layout);
					Handles.color = Color.green;
					Handles.DrawLine(car.cp, car.bp2);
					Handles.SphereHandleCap(0, car.bp2, Quaternion.identity, car.length * 0.025f, EventType.Layout);
				}
			}
		}
	}
}