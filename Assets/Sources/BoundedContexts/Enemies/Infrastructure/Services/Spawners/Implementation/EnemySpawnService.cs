using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.Frameworks.Services.ObjectPools.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Implementation
{
    public class EnemySpawnService : IEnemySpawnService
    {
        private readonly IObjectPool<EnemyView> _enemyPool;
        private readonly IEnemyViewFactory _enemyViewFactory;

        public EnemySpawnService(IObjectPool<EnemyView> enemyPool, IEnemyViewFactory enemyViewFactory)
        {
            _enemyPool = enemyPool ?? throw new ArgumentNullException(nameof(enemyPool));
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
        }

        public IEnemyView Spawn(KillEnemyCounter killEnemyCounter, Vector3 position)
        {
            Enemy enemy = new Enemy(
                new EnemyHealth(50), 
                new EnemyAttacker(0));
            
            IEnemyView enemyView = SpawnFromPool(enemy, killEnemyCounter) ?? 
                                   _enemyViewFactory.Create(enemy, killEnemyCounter);
            
            enemyView.DisableNavmeshAgent();
            enemyView.SetPosition(position);
            enemyView.EnableNavmeshAgent();
            enemyView.Show();
            
            return enemyView;
        }

        private IEnemyView SpawnFromPool(Enemy enemy, KillEnemyCounter killEnemyCounter)
        {
            EnemyView enemyView = _enemyPool.Get<EnemyView>();

            if (enemyView == null)
                return null;
            
            return _enemyViewFactory.Create(enemy, killEnemyCounter, enemyView);
        }
    }
}