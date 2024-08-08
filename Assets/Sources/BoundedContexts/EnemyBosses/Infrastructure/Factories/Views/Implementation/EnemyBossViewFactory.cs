﻿using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Prefabs;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation
{
    public class EnemyBossViewFactory
    {
        private readonly EnemyBossDependencyProviderFactory _providerFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly BurnAbilityViewFactory _burnAbilityViewFactory;
        private readonly IPoolManager _poolManager;

        public EnemyBossViewFactory(
            EnemyBossDependencyProviderFactory providerFactory,
            EnemyHealthViewFactory enemyHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            BurnAbilityViewFactory burnAbilityViewFactory,
            IPoolManager poolManager)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _burnAbilityViewFactory = burnAbilityViewFactory ?? 
                                      throw new ArgumentNullException(nameof(burnAbilityViewFactory));
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
        }

        public IEnemyBossView Create(EnemySpawner enemySpawner, Vector3 position)
        {
            BossEnemy bossEnemy = new BossEnemy(
                new EnemyHealth(enemySpawner.BossHealth), 
                new EnemyAttacker(
                    enemySpawner.BossAttackPower,
                    enemySpawner.BossMassAttackPower),
                new BurnAbility(),
                2f,
                2f,
                5f);

            EnemyBossView view = _poolManager.Get<EnemyBossView>(PrefabPath.BossEnemy);

            _providerFactory.Create(bossEnemy, view);
            _enemyHealthViewFactory.Create(bossEnemy.EnemyHealth, view.EnemyHealthView);
            _healthBarViewFactory.Create(bossEnemy.EnemyHealth, view.HealthBarView);
            _burnAbilityViewFactory.Create(bossEnemy.BurnAbility, view.BurnAbilityView);

            view.StartFsm();
            
            view.DisableNavmeshAgent();
            view.SetPosition(position);
            view.EnableNavmeshAgent();
            view.Show();

            return view;
        }
    }
}