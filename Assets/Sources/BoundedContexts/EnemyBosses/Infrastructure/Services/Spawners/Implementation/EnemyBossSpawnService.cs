using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Implementation
{
    public class EnemyBossSpawnService : IEnemyBossSpawnService
    {
        private readonly IObjectPool<Presentation.Implementation.EnemyBossView> _bossEnemyPool;
        private readonly IEnemyBossViewFactory _enemyBossViewFactory;

        public EnemyBossSpawnService(
            IObjectPool<Presentation.Implementation.EnemyBossView> bossEnemyPool, 
            IEnemyBossViewFactory enemyBossViewFactory)
        {
            _bossEnemyPool = bossEnemyPool ?? throw new ArgumentNullException(nameof(bossEnemyPool));
            _enemyBossViewFactory = enemyBossViewFactory 
                                    ?? throw new ArgumentNullException(nameof(enemyBossViewFactory));
        }

        public IEnemyBossView Spawn(KillEnemyCounter killEnemyCounter, Vector3 position)
        {
            BossEnemy bossEnemy = new BossEnemy(
                new EnemyHealth(200), 
                new EnemyAttacker(10, 30), 
                2f,
                2f,
                5f);
            IEnemyBossView enemyBossView = SpawnFromPool(bossEnemy, killEnemyCounter) ?? 
                                           _enemyBossViewFactory.Create(bossEnemy, killEnemyCounter);
            enemyBossView.DisableNavmeshAgent();
            enemyBossView.SetPosition(position);
            enemyBossView.EnableNavmeshAgent();
            enemyBossView.Show();

            return enemyBossView;
        }
        
        private IEnemyBossView SpawnFromPool(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter)
        {
            Presentation.Implementation.EnemyBossView view = 
                _bossEnemyPool.Get<Presentation.Implementation.EnemyBossView>();

            if (view == null)
                return null;
            
            return _enemyBossViewFactory.Create(bossEnemy, killEnemyCounter, view);
        }
    }
}