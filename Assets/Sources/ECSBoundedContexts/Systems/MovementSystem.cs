using Leopotam.Ecs;
using Sources.ECSBoundedContexts.Components;
using UnityEngine;

namespace Sources.ECSBoundedContexts.Systems
{
    public sealed class MovementSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private readonly EcsFilter<SearchlightTag, MoveComponent> _filter = null;
        
        public void Run()
        {
            foreach (int entity in _filter)
            {
                ref MoveComponent moveComponent = ref _filter.Get2(entity);
                
                if (moveComponent.IsFromPosition)
                    FromMove(moveComponent);
                else
                    ToMove(moveComponent);
            }
        }

        private void FromMove(MoveComponent moveComponent)
        {
            moveComponent.Transform.rotation =
                Quaternion.RotateTowards(
                    moveComponent.Transform.rotation, 
                    moveComponent.FromAngle.rotation, 
                    Time.deltaTime * moveComponent.Speed);

            float angle = Quaternion.Angle(moveComponent.Transform.rotation, moveComponent.FromAngle.rotation);
            Debug.Log(angle);

            if (Quaternion.Angle(moveComponent.Transform.rotation, moveComponent.FromAngle.rotation) <= 1f)
            {
                Debug.Log(moveComponent.IsFromPosition);
                moveComponent.IsFromPosition = false;
            }
        }
        
        private void ToMove(MoveComponent moveComponent)
        {
            moveComponent.Transform.rotation =
                Quaternion.RotateTowards(
                    moveComponent.Transform.rotation, 
                    moveComponent.ToAngle.rotation, 
                    Time.deltaTime * moveComponent.Speed);

            if (Quaternion.Angle(moveComponent.Transform.rotation, moveComponent.ToAngle.rotation) <= 0.1f)
                moveComponent.IsFromPosition = true;
        }
    }
}