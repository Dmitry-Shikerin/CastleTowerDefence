using UnityEngine;

namespace Sources.Infrastructure.Services.Linecasts
{
    public class LinecastService
    {
        public bool Linecast(Vector3 position, Vector3 colliderPosition, int obstacleLayerMask) =>
            Physics.Linecast(position, colliderPosition, obstacleLayerMask);
    }
}