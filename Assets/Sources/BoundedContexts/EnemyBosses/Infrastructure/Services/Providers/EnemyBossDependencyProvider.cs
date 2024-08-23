using System;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers
{
    public class EnemyBossDependencyProvider : MonoBehaviour
    {
        public bool IsConstructed { get; private set; }
        
        public BossEnemy BossEnemy { get; private set; }
        public KillEnemyCounter KillEnemyCounter { get; private set; }
        public PlayerWallet PlayerWallet { get; private set; }
        public IEnemyBossView View { get; private set; }
        public IEnemyBossAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public IEntityRepository EntityRepository { get; private set; }
        public ExplosionBodyBloodyViewFactory ExplosionBodyBloodyViewFactory { get; private set; }

        public void Construct(
            BossEnemy bossEnemy, 
            IEntityRepository entityRepository,
            IEnemyBossView enemyBossView, 
            IEnemyBossAnimation enemyBossAnimation,
            IOverlapService overlapService,
            ExplosionBodyBloodyViewFactory explosionBodyBloodyViewFactory)
        {
            EntityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            BossEnemy = bossEnemy ?? throw new ArgumentNullException(nameof(bossEnemy));
            KillEnemyCounter = entityRepository.Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
            PlayerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
            View = enemyBossView ?? throw new ArgumentNullException(nameof(enemyBossView));
            Animation = enemyBossAnimation ?? throw new ArgumentNullException(nameof(enemyBossAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            ExplosionBodyBloodyViewFactory = explosionBodyBloodyViewFactory ?? 
                                              throw new ArgumentNullException(nameof(explosionBodyBloodyViewFactory));
            
            IsConstructed = true;
        }
    }
}