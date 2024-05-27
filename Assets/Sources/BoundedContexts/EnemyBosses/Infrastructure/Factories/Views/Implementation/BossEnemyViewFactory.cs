using System;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.Services.ObjectPools.Generic;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation
{
    public class BossEnemyViewFactory : PoolableObjectFactory<BossEnemyView>,IBossEnemyViewFactory
    {
        private readonly BossEnemyDependencyProviderFactory _providerFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;

        public BossEnemyViewFactory(
            IObjectPool<BossEnemyView> bossEnemyPool,
            BossEnemyDependencyProviderFactory providerFactory,
            EnemyHealthViewFactory enemyHealthViewFactory) 
            : base(bossEnemyPool)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
        }

        public IBossEnemyView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter)
        {
            BossEnemyView bossEnemyView = CreateView(PrefabPath.BossEnemy);
            
            return Create(bossEnemy, killEnemyCounter, bossEnemyView);
        }
        
        public IBossEnemyView Create(
            BossEnemy bossEnemy, 
            KillEnemyCounter killEnemyCounter, 
            BossEnemyView bossEnemyView)
        {
            _providerFactory.Create(bossEnemy, killEnemyCounter, bossEnemyView);
            _enemyHealthViewFactory.Create(bossEnemy.EnemyHealth, bossEnemyView.EnemyHealthView);
            
            bossEnemyView.FsmOwner.StartBehaviour();

            return bossEnemyView;
        }
    }
}