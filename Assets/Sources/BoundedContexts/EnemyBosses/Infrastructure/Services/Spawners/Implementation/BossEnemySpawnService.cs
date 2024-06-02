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
    public class BossEnemySpawnService : IBossEnemySpawnService
    {
        private readonly IObjectPool<Presentation.Implementation.BossEnemyView> _bossEnemyPool;
        private readonly IBossEnemyViewFactory _bossEnemyViewFactory;

        public BossEnemySpawnService(
            IObjectPool<Presentation.Implementation.BossEnemyView> bossEnemyPool, 
            IBossEnemyViewFactory bossEnemyViewFactory)
        {
            _bossEnemyPool = bossEnemyPool ?? throw new ArgumentNullException(nameof(bossEnemyPool));
            _bossEnemyViewFactory = bossEnemyViewFactory 
                                    ?? throw new ArgumentNullException(nameof(bossEnemyViewFactory));
        }

        public IBossEnemyView Spawn(KillEnemyCounter killEnemyCounter, Vector3 position)
        {
            BossEnemy bossEnemy = new BossEnemy(
                new EnemyHealth(200), 
                new EnemyAttacker(10), 
                2f,
                2f,
                5f);
            IBossEnemyView bossEnemyView = SpawnFromPool(bossEnemy, killEnemyCounter) ?? 
                                           _bossEnemyViewFactory.Create(bossEnemy, killEnemyCounter);
            bossEnemyView.DisableNavmeshAgent();
            bossEnemyView.SetPosition(position);
            bossEnemyView.EnableNavmeshAgent();
            bossEnemyView.Show();

            return bossEnemyView;
        }
        
        private IBossEnemyView SpawnFromPool(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter)
        {
            Presentation.Implementation.BossEnemyView enemyView = 
                _bossEnemyPool.Get<Presentation.Implementation.BossEnemyView>();

            if (enemyView == null)
                return null;
            
            return _bossEnemyViewFactory.Create(bossEnemy, killEnemyCounter, enemyView);
        }
    }
}