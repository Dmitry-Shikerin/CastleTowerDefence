﻿using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Implementation
{
    public class EnemyKamikazeSpawnService : IEnemyKamikazeSpawnService
    {
        private readonly IObjectPool<EnemyKamikazeView> _enemyPool;
        private readonly IEnemyKamikazeViewFactory _enemyViewFactory;

        public EnemyKamikazeSpawnService(IObjectPool<EnemyKamikazeView> enemyPool, IEnemyKamikazeViewFactory enemyViewFactory)
        {
            _enemyPool = enemyPool ?? throw new ArgumentNullException(nameof(enemyPool));
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
        }

        public IEnemyKamikazeView Spawn(EnemySpawner enemySpawner, Vector3 position)
        {
            EnemyKamikaze enemy = new EnemyKamikaze(
                new EnemyHealth(enemySpawner.KamikazeHealth), 
                new EnemyAttacker(
                    enemySpawner.KamikazeAttackPower,
                    enemySpawner.KamikazeMassAttackPower),
                new BurnAbility());
            
            IEnemyKamikazeView enemyView = SpawnFromPool(enemy) ?? 
                                   _enemyViewFactory.Create(enemy);
            
            enemyView.DisableNavmeshAgent();
            enemyView.SetPosition(position);
            enemyView.EnableNavmeshAgent();
            enemyView.Show();
            
            return enemyView;
        }

        private IEnemyKamikazeView SpawnFromPool(EnemyKamikaze enemy)
        {
            EnemyKamikazeView enemyView = _enemyPool.Get<EnemyKamikazeView>();

            if (enemyView == null)
                return null;
            
            return _enemyViewFactory.Create(enemy, enemyView);
        }
    }
}