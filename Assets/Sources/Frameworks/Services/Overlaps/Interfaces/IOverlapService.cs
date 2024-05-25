using System.Collections.Generic;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Services.Overlaps
{
    public interface IOverlapService
    {
        IReadOnlyList<T> OverlapSphere<T>(
            Vector3 position, float radius, int searchLayerMask, int obstacleLayerMask)
            where T : MonoBehaviour;
    }
}