using System;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers
{
    public class EnemyBossDependencyProvider : MonoBehaviour
    {
        private bool _isConstructed;
        
        public BossEnemy BossEnemy { get; private set; }
        public KillEnemyCounter KillEnemyCounter { get; private set; }
        public IEnemyBossView View { get; private set; }
        public IEnemyBossAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public IExplosionBodyBloodySpawnService ExplosionBodyBloodySpawnService { get; private set; }

        public void Construct(
            BossEnemy bossEnemy, 
            KillEnemyCounter killEnemyCounter, 
            IEnemyBossView enemyBossView, 
            IEnemyBossAnimation enemyBossAnimation,
            IOverlapService overlapService,
            IExplosionBodyBloodySpawnService explosionBodyBloodySpawnService)
        {
            BossEnemy = bossEnemy ?? throw new ArgumentNullException(nameof(bossEnemy));
            KillEnemyCounter = killEnemyCounter ?? throw new ArgumentNullException(nameof(killEnemyCounter));
            View = enemyBossView ?? throw new ArgumentNullException(nameof(enemyBossView));
            Animation = enemyBossAnimation ?? throw new ArgumentNullException(nameof(enemyBossAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            ExplosionBodyBloodySpawnService = explosionBodyBloodySpawnService ?? 
                                              throw new ArgumentNullException(nameof(explosionBodyBloodySpawnService));
            
            _isConstructed = true;
        }
    }
}