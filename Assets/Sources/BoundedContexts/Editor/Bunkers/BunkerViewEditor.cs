using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Editor.Bunkers
{
    [CustomEditor(typeof(SpawnPoint))]
    public class BunkerViewEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(BunkerView spawner, GizmoType gizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
    }
}