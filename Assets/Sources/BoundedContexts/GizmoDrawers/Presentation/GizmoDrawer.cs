using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.GizmoDrawers.Presentation
{
    public class GizmoDrawerView : MonoBehaviour
    {
        [FormerlySerializedAs("_range")] [SerializeField] private float _radius;
        
        private void OnDrawGizmos()
        {
            DrawGizmos();
        }
        
        private void OnDrawGizmosSelected()
        {
            
        }
        
        private void DrawGizmos()
        {
            Gizmos.color = new Color(255, 0, 0, 0.5f);
            Gizmos.DrawSphere(gameObject.transform.position, _radius);
        }
    }
}