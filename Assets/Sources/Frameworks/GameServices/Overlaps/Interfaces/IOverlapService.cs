using System.Collections.Generic;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Overlaps.Interfaces
{
    public interface IOverlapService
    {
        IReadOnlyList<T> OverlapSphere<T>(
            Vector3 position, float radius, int searchLayerMask, int obstacleLayerMask = 0)
            where T : MonoBehaviour;
    }
}