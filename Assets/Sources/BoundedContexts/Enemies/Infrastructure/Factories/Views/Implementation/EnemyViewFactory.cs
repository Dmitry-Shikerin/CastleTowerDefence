using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Domain.Constants;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation
{
    public class EnemyViewFactory
    {
        private readonly EnemyDependencyProviderFactory _providerFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly BurnAbilityViewFactory _burnAbilityViewFactory;
        private readonly IPoolManager _poolManager;

        public EnemyViewFactory(
            EnemyDependencyProviderFactory providerFactory,
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
        
        public IEnemyView Create(EnemySpawner enemySpawner, Vector3 position)
        {
            Enemy enemy = new Enemy(
                new EnemyHealth(enemySpawner.EnemyHealth), 
                new EnemyAttacker(
                    enemySpawner.EnemyAttackPower, 
                    0),
                new BurnAbility());
            
            EnemyView view = _poolManager.Get<EnemyView>(EnemyConst.PrefabPath);
            
            _providerFactory.Create(enemy, view);
            _enemyHealthViewFactory.Create(enemy.EnemyHealth, view.EnemyHealthView);
            _healthBarViewFactory.Create(enemy.EnemyHealth, view.HealthBarView);
            _burnAbilityViewFactory.Create(enemy.BurnAbility, view.BurnAbilityView);
            
            view.StartFsm();
            
            view.DisableNavmeshAgent();
            view.SetPosition(position);
            view.EnableNavmeshAgent();
            view.Show();

            return view;
            // EnemyView enemyView = CreateView(EnemyConst.PrefabPath);
            //
            // return Create(enemy, enemyView);
        }

        public IEnemyView Create(Enemy enemy, EnemyView view)
        {
            _providerFactory.Create(enemy, view);
            _enemyHealthViewFactory.Create(enemy.EnemyHealth, view.EnemyHealthView);
            _healthBarViewFactory.Create(enemy.EnemyHealth, view.HealthBarView);
            _burnAbilityViewFactory.Create(enemy.BurnAbility, view.BurnAbilityView);
            
            view.StartFsm();
            
            return view;
        }
    }
}