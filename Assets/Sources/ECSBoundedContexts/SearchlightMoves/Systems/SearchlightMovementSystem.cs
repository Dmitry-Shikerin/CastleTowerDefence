using Leopotam.Ecs;
using Sources.ECSBoundedContexts.SearchlightMoves.Components;
using UnityEngine;

namespace Sources.ECSBoundedContexts.SearchlightMoves.Systems
{
    public sealed class SearchlightMovementSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SearchlightTag, SearchlightMoveComponent> _filter = null;
        
        public void Run()
        {
            foreach (int entity in _filter)
            {
                ref SearchlightMoveComponent searchlightMoveComponent = ref _filter.Get2(entity);

                Quaternion from = searchlightMoveComponent.FromAngle.rotation;
                Quaternion to = searchlightMoveComponent.ToAngle.rotation;
                
                if (searchlightMoveComponent.IsFromPosition)
                    Move(ref searchlightMoveComponent, from, false);
                else
                    Move(ref searchlightMoveComponent, to, true);
            }
        }

        private void Move(ref SearchlightMoveComponent searchlightMoveComponent, Quaternion target, bool isFromPosition)
        {
            float angle = Quaternion.Angle(
                searchlightMoveComponent.Transform.rotation, target);
                    
            if (angle <= Quaternion.kEpsilon)
                searchlightMoveComponent.IsFromPosition = isFromPosition;
            
            searchlightMoveComponent.Transform.rotation =
                Quaternion.RotateTowards(
                    searchlightMoveComponent.Transform.rotation, 
                    target, 
                    Time.deltaTime * searchlightMoveComponent.Speed);
        }
    }
}