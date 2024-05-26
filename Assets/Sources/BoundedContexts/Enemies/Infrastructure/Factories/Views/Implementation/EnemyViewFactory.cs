using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.Services.ObjectPools;
using Sources.Frameworks.Services.ObjectPools.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation
{
    public class EnemyViewFactory : IEnemyViewFactory
    {
        private readonly EnemyDependencyProviderFactory _providerFactory;
        private readonly IObjectPool<EnemyView> _enemyPool;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;

        public EnemyViewFactory(
            EnemyDependencyProviderFactory providerFactory,
            IObjectPool<EnemyView> enemyPool,
            EnemyHealthViewFactory enemyHealthViewFactory)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _enemyPool = enemyPool ?? throw new ArgumentNullException(nameof(enemyPool));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
        }
        
        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter)
        {
            EnemyView enemyView = CreateView();
            
            return Create(enemy, killEnemyCounter, enemyView);
        }

        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter, EnemyView view)
        {
            _providerFactory.Create(enemy, view);
            view.FsmOwner.StartBehaviour();
            
            _enemyHealthViewFactory.Create(enemy.EnemyHealth, view.EnemyHealthView);
            
            return view;
        }

        private EnemyView CreateView()
        {
            EnemyView enemyView = Object.Instantiate(
                Resources.Load<EnemyView>(PrefabPath.Enemy));

            enemyView
                .AddComponent<PoolableObject>()
                .SetPool(_enemyPool);

            return enemyView;
        }
    }
}