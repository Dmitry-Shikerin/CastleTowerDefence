using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers
{
    public class EnemyDependencyProviderFactory
    {
        private readonly IOverlapService _overlapService;

        public EnemyDependencyProviderFactory(IOverlapService overlapService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
        }

        public EnemyDependencyProvider Create(Enemy enemy, EnemyView view)
        {
            EnemyDependencyProvider provider = view.Provider;
            provider.Construct(
                enemy, 
                view,
                view.Animation,
                _overlapService);
            
            return provider;
        }
    }
}