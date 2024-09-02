using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.Utils.Injects;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation
{
    public class EnemyViewFactory
    {
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly BurnAbilityViewFactory _burnAbilityViewFactory;
        private readonly IPoolManager _poolManager;
        private readonly DiContainer _container;

        public EnemyViewFactory(
            EnemyHealthViewFactory enemyHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            BurnAbilityViewFactory burnAbilityViewFactory,
            IPoolManager poolManager,
            DiContainer container)
        {
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _burnAbilityViewFactory = burnAbilityViewFactory ?? 
                                      throw new ArgumentNullException(nameof(burnAbilityViewFactory));
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        
        public IEnemyView Create(EnemySpawner enemySpawner, Vector3 position)
        {
            Enemy enemy = new Enemy(
                new EnemyHealth(enemySpawner.EnemyHealth), 
                new EnemyAttacker(
                    enemySpawner.EnemyAttackPower, 
                    0),
                new BurnAbility());
            
            EnemyView view = _poolManager.Get<EnemyView>();
            
            _enemyHealthViewFactory.Create(enemy.EnemyHealth, view.EnemyHealthView);
            _healthBarViewFactory.Create(enemy.EnemyHealth, view.HealthBarView);
            _burnAbilityViewFactory.Create(enemy.BurnAbility, view.BurnAbilityView);
            
            view.FsmOwner.ConstructFsm(enemy, view);
            view.FsmOwner.InjectFsm(_container);
            _container.Inject(view.LookAtCamera);
            view.StartFsm();
            
            view.DisableNavmeshAgent();
            view.SetPosition(position);
            view.EnableNavmeshAgent();
            view.Show();

            return view;
        }
    }
}