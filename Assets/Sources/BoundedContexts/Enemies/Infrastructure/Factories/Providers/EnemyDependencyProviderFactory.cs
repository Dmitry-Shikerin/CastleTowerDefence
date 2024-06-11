using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers
{
    public class EnemyDependencyProviderFactory
    {
        private readonly IOverlapService _overlapService;
        private readonly IExplosionBodyBloodySpawnService _explosionBodyBloodySpawnService;

        public EnemyDependencyProviderFactory(
            IOverlapService overlapService,
            IExplosionBodyBloodySpawnService explosionBodyBloodySpawnService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _explosionBodyBloodySpawnService = 
                explosionBodyBloodySpawnService 
                ?? throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
        }

        public EnemyDependencyProvider Create(
            Enemy enemy, 
            PlayerWallet playerWallet, 
            EnemyView view)
        {
            EnemyDependencyProvider provider = view.Provider;
            provider.Construct(
                enemy, 
                playerWallet,
                view,
                view.Animation,
                _overlapService,
                _explosionBodyBloodySpawnService);
            
            return provider;
        }
    }
}