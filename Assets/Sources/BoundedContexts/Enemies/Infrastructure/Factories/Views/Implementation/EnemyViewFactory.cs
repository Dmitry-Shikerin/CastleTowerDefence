using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Constants;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.Services.ObjectPools.Generic;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation
{
    public class EnemyViewFactory : PoolableObjectFactory<EnemyView>,IEnemyViewFactory
    {
        private readonly EnemyDependencyProviderFactory _providerFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;

        public EnemyViewFactory(
            EnemyDependencyProviderFactory providerFactory,
            IObjectPool<EnemyView> enemyPool,
            EnemyHealthViewFactory enemyHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory) 
            : base(enemyPool)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
        }
        
        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter)
        {
            EnemyView enemyView = CreateView(EnemyConst.PrefabPath);
            
            return Create(enemy, killEnemyCounter, enemyView);
        }

        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter, EnemyView view)
        {
            _providerFactory.Create(enemy, view);
            view.FsmOwner.StartBehaviour();
            
            _enemyHealthViewFactory.Create(enemy.EnemyHealth, view.EnemyHealthView);
            _healthBarViewFactory.Create(enemy.EnemyHealth, view.HealthBarView);
            
            return view;
        }
    }
}