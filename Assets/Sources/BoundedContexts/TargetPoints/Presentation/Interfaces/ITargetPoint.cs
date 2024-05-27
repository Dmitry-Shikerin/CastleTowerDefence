using Sources.BoundedContexts.TargetPoints.Presentation.Implementation.Types;
using UnityEngine;

namespace Sources.BoundedContexts.TargetPoints.Presentation.Interfaces
{
    public interface ITargetPoint
    {
        TargetPointType Type { get; }
        Vector3 Position { get; }
    }
}