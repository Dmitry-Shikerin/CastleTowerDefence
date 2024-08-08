using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Prefabs;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Implementation
{
    public class EnemyKamikazeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly EnemyKamikazeDependencyProviderFactory _dependencyProviderFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly BurnAbilityViewFactory _burnAbilityViewFactory;

        public EnemyKamikazeViewFactory(
            IPoolManager poolManager,
            EnemyKamikazeDependencyProviderFactory dependencyProviderFactory,
            EnemyHealthViewFactory enemyHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            BurnAbilityViewFactory burnAbilityViewFactory) 
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _dependencyProviderFactory = dependencyProviderFactory ?? 
                                         throw new ArgumentNullException(nameof(dependencyProviderFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _burnAbilityViewFactory = burnAbilityViewFactory ?? 
                                      throw new ArgumentNullException(nameof(burnAbilityViewFactory));
        }
        
        public IEnemyKamikazeView Create(EnemySpawner enemySpawner, Vector3 position)
        {
            EnemyKamikaze enemy = new EnemyKamikaze(
                new EnemyHealth(enemySpawner.KamikazeHealth), 
                new EnemyAttacker(
                    enemySpawner.KamikazeAttackPower,
                    enemySpawner.KamikazeMassAttackPower),
                new BurnAbility());
            
            EnemyKamikazeView enemyView = _poolManager.Get<EnemyKamikazeView>(PrefabPath.EnemyKamikaze);
            
            enemyView.DisableNavmeshAgent();
            enemyView.SetPosition(position);
            enemyView.EnableNavmeshAgent();
            enemyView.Show();
            
            
            return Create(enemy, enemyView);
        }

        public IEnemyKamikazeView Create(EnemyKamikaze enemy, EnemyKamikazeView view)
        {
            _dependencyProviderFactory.Create(enemy, view);
            _enemyHealthViewFactory.Create(enemy.EnemyHealth, view.EnemyHealthView);
            _healthBarViewFactory.Create(enemy.EnemyHealth, view.HealthBarView);
            _burnAbilityViewFactory.Create(enemy.BurnAbility, view.BurnAbilityView);
            
            view.StartFsm();
            
            return view;
        }
    }
}