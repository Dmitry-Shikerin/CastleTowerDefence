using System;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers
{
    public class EnemyDependencyProviderFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IOverlapService _overlapService;
        private readonly ExplosionBodyBloodyViewFactory _explosionBodyBloodyViewFactory;

        public EnemyDependencyProviderFactory(
            IEntityRepository entityRepository,
            IOverlapService overlapService,
            ExplosionBodyBloodyViewFactory explosionBodyBloodyViewFactory)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _explosionBodyBloodyViewFactory = 
                explosionBodyBloodyViewFactory 
                ?? throw new ArgumentNullException(nameof(explosionBodyBloodyViewFactory));
        }

        public EnemyDependencyProvider Create(Enemy enemy, EnemyView view)
        {
            EnemyDependencyProvider provider = view.Provider;
            provider.Construct(
                enemy, 
                _entityRepository,
                view,
                view.Animation,
                _overlapService,
                _explosionBodyBloodyViewFactory);
            
            return provider;
        }
    }
}