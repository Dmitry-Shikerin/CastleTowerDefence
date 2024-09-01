using System;
using Sources.BoundedContexts.BurnAbilities.Domain;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.Utils.Injects;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views
{
    public class EnemyKamikazeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly BurnAbilityViewFactory _burnAbilityViewFactory;
        private readonly DiContainer _container;

        public EnemyKamikazeViewFactory(
            IPoolManager poolManager,
            EnemyHealthViewFactory enemyHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            BurnAbilityViewFactory burnAbilityViewFactory,
            DiContainer container) 
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _burnAbilityViewFactory = burnAbilityViewFactory ?? 
                                      throw new ArgumentNullException(nameof(burnAbilityViewFactory));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        
        public IEnemyKamikazeView Create(EnemySpawner enemySpawner, Vector3 position)
        {
            Enemy enemy = new Enemy(
                new EnemyHealth(enemySpawner.KamikazeHealth), 
                new EnemyAttacker(
                    enemySpawner.KamikazeAttackPower,
                    enemySpawner.KamikazeMassAttackPower),
                new BurnAbility());
            
            EnemyKamikazeView view = _poolManager.Get<EnemyKamikazeView>();
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