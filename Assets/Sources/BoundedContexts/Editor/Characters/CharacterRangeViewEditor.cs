using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using UnityEditor;
using UnityEngine;

namespace Sources.BoundedContexts.Editor.Characters
{
    [CustomEditor(typeof(CharacterRangeView))]
    public class CharacterRangeViewEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.Selected)]
        public static void RenderCustomGizmo(CharacterRangeView characterRangeView, GizmoType gizmo)
        {
            Gizmos.color = Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawSphere(characterRangeView.transform.position, characterRangeView.FindRange);
        }
    }
}