﻿using UnityEngine;

namespace Sources.Frameworks.Services.Linecasts.Interfaces
{
    public interface ILinecastService
    {
        bool Linecast(Vector3 position, Vector3 colliderPosition, int obstacleLayerMask);
    }
}