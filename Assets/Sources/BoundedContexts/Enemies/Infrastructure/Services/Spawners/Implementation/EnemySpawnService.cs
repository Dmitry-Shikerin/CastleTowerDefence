using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
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

        public IEnemyView Spawn(
            KillEnemyCounter killEnemyCounter, 
            EnemySpawner enemySpawner, 
            Vector3 position)
        {
            Enemy enemy = new Enemy(
                new EnemyHealth(enemySpawner.EnemyHealth), 
                new EnemyAttacker(
                    enemySpawner.EnemyAttackPower, 
                           0),
                new BurnAbility());
            
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