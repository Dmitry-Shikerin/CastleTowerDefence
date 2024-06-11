using System;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Providers
{
    public class EnemyKamikazeDependencyProviderFactory
    {
        private readonly IOverlapService _overlapService;
        private readonly IExplosionBodySpawnService _explosionBodySpawnService;
        private readonly IExplosionBodyBloodySpawnService _explosionBodyBloodySpawnService;

        public EnemyKamikazeDependencyProviderFactory(
            IOverlapService overlapService,
            IExplosionBodySpawnService explosionBodySpawnService,
            IExplosionBodyBloodySpawnService explosionBodyBloodySpawnService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _explosionBodySpawnService = explosionBodySpawnService ?? 
                                         throw new ArgumentNullException(nameof(explosionBodySpawnService));
            _explosionBodyBloodySpawnService = explosionBodyBloodySpawnService ??
                                               throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
        }

        public EnemyKamikazeDependencyProvider Create(
            EnemyKamikaze enemyKamikaze, 
            KillEnemyCounter killEnemyCounter,
            PlayerWallet playerWallet,
            IEnemyKamikazeView view)
        {
            view.Provider.Construct(
                enemyKamikaze, 
                killEnemyCounter,
                playerWallet,
                view,
                view.Animation,
                _overlapService,
                _explosionBodySpawnService,
                _explosionBodyBloodySpawnService);
            
            return view.Provider;
        }
    }
}