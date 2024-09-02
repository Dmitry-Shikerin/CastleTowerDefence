using Sources.Frameworks.GameServices.Linecasts.Interfaces;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Linecasts.Implementation
{
    public class LinecastService : ILinecastService
    {
        public bool Linecast(Vector3 position, Vector3 colliderPosition, int obstacleLayerMask) =>
            Physics.Linecast(position, colliderPosition, obstacleLayerMask);
    }
}