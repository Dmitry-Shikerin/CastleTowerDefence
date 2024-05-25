using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Editor.SpawnPoints
{
    [CustomEditor(typeof(SpawnPoint))]
    public class SpawnPointEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnPoint spawner, GizmoType gizmo)
        {
            Gizmos.color = SetColor(spawner);
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }

        private static Color SetColor(SpawnPoint spawnPoint)
        {
            return  spawnPoint.Type switch
            {
                SpawnPointType.Character => Color.green,
                SpawnPointType.Enemy => Color.red,
                _ => Color.white
            };
        }
    }
}