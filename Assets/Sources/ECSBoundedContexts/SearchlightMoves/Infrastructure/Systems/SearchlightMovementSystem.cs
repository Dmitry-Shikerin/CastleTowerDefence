using Leopotam.Ecs;
using Sources.ECSBoundedContexts.SearchlightMoves.Domain.Components;
using UnityEngine;
using Zenject;

namespace Sources.ECSBoundedContexts.SearchlightMoves.Infrastructure.Systems
{
    public sealed class SearchlightMovementSystem : IEcsRunSystem
    {
        private readonly DiContainer _diContainer = null;
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SearchlightTag, SearchlightMoveComponent> _filter = null;

        public void Run()
        {
            foreach (int entity in _filter)
            {
                ref SearchlightMoveComponent moveComponent = ref _filter.Get2(entity);

                float from = moveComponent.Angle.x;
                float to = moveComponent.Angle.y;

                if (moveComponent.IsFromPosition)
                    Move(ref moveComponent, from, false);
                else
                    Move(ref moveComponent, to, true);
            }
        }

        private void Move(ref SearchlightMoveComponent moveComponent, float targetAngle, bool isFromPosition)
        {
            float xAngle = moveComponent.Transform.rotation.x;
            float yAngle = moveComponent.Transform.rotation.y;
            Quaternion targetRotation = Quaternion.Euler(xAngle, targetAngle, yAngle);
            Quaternion currentRotation = moveComponent.Transform.rotation;
            float delta = Time.deltaTime * moveComponent.Speed;
            
            float angleDifference = Quaternion.Angle(currentRotation, targetRotation);

            if (angleDifference <= Quaternion.kEpsilon)
                moveComponent.IsFromPosition = isFromPosition;

            moveComponent.Transform.rotation = Quaternion.RotateTowards(
                moveComponent.Transform.rotation, targetRotation, delta);
        }
    }
}