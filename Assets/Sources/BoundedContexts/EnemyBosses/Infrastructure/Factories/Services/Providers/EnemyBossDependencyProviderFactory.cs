using System;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers
{
    public class EnemyBossDependencyProviderFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IOverlapService _overlapService;
        private readonly IExplosionBodyBloodySpawnService _explosionBodyBloodySpawnService;

        public EnemyBossDependencyProviderFactory(
            IEntityRepository entityRepository,
            IOverlapService overlapService,
            IExplosionBodyBloodySpawnService explosionBodyBloodySpawnService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _explosionBodyBloodySpawnService = explosionBodyBloodySpawnService ?? 
                                               throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
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
                _explosionBodyBloodySpawnService);

            return provider;
        }
    }
}