using System;
using Sources.BoundedContexts.BossEnemyView.Domain;
using Sources.BoundedContexts.BossEnemyView.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.Services.ObjectPools;
using Sources.Frameworks.Services.ObjectPools.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.BoundedContexts.BossEnemyView.Infrastructure.Factories.Views.Implementation
{
    public class BossEnemyViewFactory
    {
        private readonly IObjectPool<Presentation.Implementation.BossEnemyView> _bossEnemyPool;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;

        public BossEnemyViewFactory(
            IObjectPool<Presentation.Implementation.BossEnemyView> bossEnemyPool,
            EnemyHealthViewFactory enemyHealthViewFactory)
        {
            _bossEnemyPool = bossEnemyPool ?? throw new ArgumentNullException(nameof(bossEnemyPool));
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
            Presentation.Implementation.BossEnemyView bossEnemyView)
        {
            _enemyHealthViewFactory.Create(bossEnemy.EnemyHealth, bossEnemyView.EnemyHealthView);

            return bossEnemyView;
        }

        private Presentation.Implementation.BossEnemyView CreateView()
        {
            Presentation.Implementation.BossEnemyView bossEnemyView = Object.Instantiate(
                Resources.Load<Presentation.Implementation.BossEnemyView>(PrefabPath.BossEnemy));

            bossEnemyView
                .AddComponent<PoolableObject>()
                .SetPool(_bossEnemyPool);

            return bossEnemyView;
        }
    }
}