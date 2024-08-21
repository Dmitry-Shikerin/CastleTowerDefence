using UnityEngine;
using UnityEditor;

namespace MegaFiers
{
	[CustomEditor(typeof(MegaTankWheels))]
	public class MegaTankWheelsEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		}

		[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.Pickable | GizmoType.InSelectionHierarchy)]
		static void RenderGizmo(MegaTankWheels track, GizmoType gizmoType)
		{
			if ( (gizmoType & GizmoType.Active) != 0 && Selection.activeObject == track.gameObject )
			{
				Gizmos.matrix = track.transform.localToWorldMatrix;
				Gizmos.DrawWireSphere(Vector3.zero, track.radius);
			}
		}
	}
}