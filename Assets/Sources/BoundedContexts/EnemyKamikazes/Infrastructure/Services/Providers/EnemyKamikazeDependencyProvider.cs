using System;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers
{
    public class EnemyKamikazeDependencyProvider : MonoBehaviour
    {
        public EnemyKamikaze EnemyKamikaze { get; private set; }
        public PlayerWallet PlayerWallet { get; private set; }
        public KillEnemyCounter KillEnemyCounter { get; private set; }
        public IEnemyKamikazeView View { get; private set; }
        public IEnemyAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public IExplosionBodySpawnService ExplosionBodySpawnService { get; private set; }
        public IExplosionBodyBloodySpawnService ExplosionBodyBloodySpawnService { get; private set; }

        public void Construct(
            EnemyKamikaze enemyKamikaze,
            IEntityRepository entityRepository,
            IEnemyKamikazeView view,
            IEnemyAnimation enemyAnimation,
            IOverlapService overlapService,
            IExplosionBodySpawnService explosionBodySpawnService,
            IExplosionBodyBloodySpawnService explosionBodyBloodySpawnService)
        {
            if (entityRepository == null) 
                throw new ArgumentNullException(nameof(entityRepository));
            
            EnemyKamikaze = enemyKamikaze ?? throw new ArgumentNullException(nameof(enemyKamikaze));
            View = view ?? throw new ArgumentNullException(nameof(view));
            Animation = enemyAnimation ?? throw new ArgumentNullException(nameof(enemyAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            ExplosionBodySpawnService = explosionBodySpawnService ?? 
                                         throw new ArgumentNullException(nameof(explosionBodySpawnService));
            ExplosionBodyBloodySpawnService = explosionBodyBloodySpawnService ?? 
                                              throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
            KillEnemyCounter = entityRepository.Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            PlayerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
        }
    }
}