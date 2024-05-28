using Sources.Frameworks.Services.Linecasts.Interfaces;
using UnityEngine;

namespace Sources.Frameworks.Services.Linecasts
{
    public class LinecastService : ILinecastService
    {
        public bool Linecast(Vector3 position, Vector3 colliderPosition, int obstacleLayerMask) =>
            Physics.Linecast(position, colliderPosition, obstacleLayerMask);
    }
}