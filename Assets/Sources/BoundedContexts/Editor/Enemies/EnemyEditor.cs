using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.Enemies.Presentation;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Editor.Enemies
{
    [CustomEditor(typeof(CharacterRangeView))]
    public class EnemyEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.Selected)]
        public static void RenderCustomGizmo(EnemyView enemyView, GizmoType gizmo)
        {
            Gizmos.color = Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawSphere(enemyView.transform.position, enemyView.FindRange);
        }
    }
}