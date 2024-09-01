using System;
using System.Collections.Generic;
using Sources.Frameworks.GameServices.Linecasts.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Overlaps.Implementation
{
    public class OverlapService : IOverlapService
    {
        private readonly Collider[] _colliders = new Collider[32];
        private readonly Collider[] _boxColliders = new Collider[150];
        private readonly ILinecastService _linecastService;

        public OverlapService(ILinecastService linecastService)
        {
            _linecastService = linecastService ?? throw new ArgumentNullException(nameof(linecastService));
        }

        public IReadOnlyList<T> OverlapSphere<T>(
            Vector3 position, float radius, int searchLayerMask, int obstacleLayerMask = 0)
            where T : MonoBehaviour
        {
            int collidersCount = OverlapSphere(position, radius, searchLayerMask);

            if (collidersCount == 0)
                return new List<T>();

            return Filter<T>(position, _colliders, obstacleLayerMask, collidersCount);
        }

        private IReadOnlyList<T> Filter<T>(
            Vector3 position, Collider[] colliders, int obstacleLayerMask, int collidersCount)
            where T : MonoBehaviour
        {
            List<T> components = new List<T>();

            for (var i = 0; i < collidersCount; i++)
            {
                Collider collider = colliders[i];

                if (collider.TryGetComponent(out T component) == false)
                    continue;

                Vector3 colliderPosition = collider.transform.position;

                if (_linecastService.Linecast(position, colliderPosition, obstacleLayerMask))
                    continue;

                components.Add(component);
            }

            return components;
        }

        private int OverlapSphere(Vector3 position, float radius, int searchLayerMask) =>
            Physics.OverlapSphereNonAlloc(position, radius, _colliders, searchLayerMask);

        public IReadOnlyList<T> OverlapBox<T>(
            Vector3 position, 
            Vector3 square, 
            int searchLayerMask, 
            int obstacleLayerMask = 0) 
            where T : MonoBehaviour
        {
            int collidersCount = OverlapBox(position, square, searchLayerMask);
            
            if (collidersCount == 0)
                return new List<T>();

            return Filter<T>(position, _boxColliders, obstacleLayerMask, collidersCount);
        }
        
        private int OverlapBox(Vector3 position, Vector3 square, int searchLayerMask) =>
            Physics.OverlapBoxNonAlloc(
                position, square, _boxColliders, Quaternion.identity, searchLayerMask);
    }
}