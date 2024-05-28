using Sources.BoundedContexts.CharacterMelees.Presentation;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Editor.AttackTargetFinders
{
    [CustomEditor(typeof(CharacterMeleeView))]
    public class AttackTargetFinderEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(CharacterMeleeView characterMeleeView, GizmoType gizmo)
        {
            Gizmos.color = Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawSphere(characterMeleeView.transform.position, characterMeleeView.FindRange);
        }
    }
}