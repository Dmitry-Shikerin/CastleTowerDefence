using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Editor.Characters
{
    [CustomEditor(typeof(CharacterMeleeView))]
    public class CharacterMeleeEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(CharacterMeleeView characterMeleeView, GizmoType gizmo)
        {
            Gizmos.color = Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawSphere(characterMeleeView.transform.position, characterMeleeView.FindRange);
        }
    }
}