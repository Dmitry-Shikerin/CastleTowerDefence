using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using Sources.BoundedContexts.TargetPoints.Presentation.Implementation;
using Sources.BoundedContexts.TargetPoints.Presentation.Implementation.Types;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Editor.TargetPoints
{
    [CustomEditor(typeof(TargetPoint))]
    public class TargetPointEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(TargetPoint targetPoint, GizmoType gizmo)
        {
            Gizmos.color = SetColor(targetPoint);
            Gizmos.DrawSphere(targetPoint.transform.position, 0.5f);
        }

        private static Color SetColor(TargetPoint spawnPoint)
        {
            return spawnPoint.Type switch
            {
                TargetPointType.Enemy => Color.yellow,
                _ => Color.white
            };
        }
    }
}