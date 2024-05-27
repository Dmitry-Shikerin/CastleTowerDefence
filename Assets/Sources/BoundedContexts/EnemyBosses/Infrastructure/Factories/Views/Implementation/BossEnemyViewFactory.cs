using System;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.Services.ObjectPools;
using Sources.Frameworks.Services.ObjectPools.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation
{
    public class BossEnemyViewFactory : IBossEnemyViewFactory
    {
        private readonly IObjectPool<EnemyBosses.Presentation.Implementation.BossEnemyView> _bossEnemyPool;
        private readonly BossEnemyDependencyProviderFactory _providerFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;

        public BossEnemyViewFactory(
            IObjectPool<EnemyBosses.Presentation.Implementation.BossEnemyView> bossEnemyPool,
            BossEnemyDependencyProviderFactory providerFactory,
            EnemyHealthViewFactory enemyHealthViewFactory)
        {
            _bossEnemyPool = bossEnemyPool ?? throw new ArgumentNullException(nameof(bossEnemyPool));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
        }

        public IBossEnemyView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter)
        {
            Presentation.Implementation.BossEnemyView bossEnemyView = CreateView();
            
            return Create(bossEnemy, killEnemyCounter, bossEnemyView);
        }
        
        public IBossEnemyView Create(
            BossEnemy bossEnemy, 
            KillEnemyCounter killEnemyCounter, 
            EnemyBosses.Presentation.Implementation.BossEnemyView bossEnemyView)
        {
            _providerFactory.Create(bossEnemy, killEnemyCounter, bossEnemyView);
            _enemyHealthViewFactory.Create(bossEnemy.EnemyHealth, bossEnemyView.EnemyHealthView);
            
            bossEnemyView.FsmOwner.StartBehaviour();

            return bossEnemyView;
        }

        private EnemyBosses.Presentation.Implementation.BossEnemyView CreateView()
        {
            EnemyBosses.Presentation.Implementation.BossEnemyView bossEnemyView = Object.Instantiate(
                Resources.Load<EnemyBosses.Presentation.Implementation.BossEnemyView>(PrefabPath.BossEnemy));

            bossEnemyView
                .AddComponent<PoolableObject>()
                .SetPool(_bossEnemyPool);

            return bossEnemyView;
        }
    }
}