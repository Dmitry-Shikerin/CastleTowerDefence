using System;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers
{
    public class EnemyBossDependencyProviderFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IOverlapService _overlapService;
        private readonly ExplosionBodyBloodyViewFactory _explosionBodyBloodyViewFactory;

        public EnemyBossDependencyProviderFactory(
            IEntityRepository entityRepository,
            IOverlapService overlapService,
            ExplosionBodyBloodyViewFactory explosionBodyBloodyViewFactory)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _explosionBodyBloodyViewFactory = explosionBodyBloodyViewFactory ?? 
                                               throw new ArgumentNullException(nameof(explosionBodyBloodyViewFactory));
        }

        public EnemyBossDependencyProvider Create(BossEnemy bossEnemy, IEnemyBossView enemyBossView)
        {
            EnemyBossDependencyProvider provider = enemyBossView.Provider;
            provider.Construct(
                bossEnemy, 
                _entityRepository,
                enemyBossView, 
                enemyBossView.Animation,
                _overlapService,
                _explosionBodyBloodyViewFactory);

            return provider;
        }
    }
}