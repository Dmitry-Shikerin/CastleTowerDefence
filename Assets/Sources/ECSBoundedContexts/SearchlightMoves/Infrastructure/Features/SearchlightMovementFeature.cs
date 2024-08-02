using Leopotam.Ecs;
using Sources.ECSBoundedContexts.SearchlightMoves.Infrastructure.Systems;
using UnityEngine;
using Zenject;

namespace Sources.ECSBoundedContexts.SearchlightMoves.Infrastructure.Features
{
    public class SearchlightMovementFeature : IEcsFeature
    {
        
        public void Bind(EcsSystems systems)
        {
            systems
                .Add(new SearchlightMovementSystem());
        }
    }
}