using Sources.BoundedContexts.TargetPoints.Presentation.Implementation.Types;
using Sources.BoundedContexts.TargetPoints.Presentation.Interfaces;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.TargetPoints.Presentation.Implementation
{
    public class TargetPoint : View, ITargetPoint
    {
        [SerializeField] private TargetPointType _type;

        public TargetPointType Type => _type;
        public Vector3 Position => transform.position;
    }
}