using System;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.BoundedContexts.Prefabs;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation
{
    public class EnemyBossViewFactory : PoolableObjectFactory<EnemyBossView>,IEnemyBossViewFactory
    {
        private readonly EnemyBossDependencyProviderFactory _providerFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;

        public EnemyBossViewFactory(
            IObjectPool<EnemyBossView> bossEnemyPool,
            EnemyBossDependencyProviderFactory providerFactory,
            EnemyHealthViewFactory enemyHealthViewFactory) 
            : base(bossEnemyPool)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
        }

        public IEnemyBossView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter)
        {
            EnemyBossView enemyBossView = CreateView(PrefabPath.BossEnemy);
            
            return Create(bossEnemy, killEnemyCounter, enemyBossView);
        }
        
        public IEnemyBossView Create(
            BossEnemy bossEnemy, 
            KillEnemyCounter killEnemyCounter, 
            EnemyBossView enemyBossView)
        {
            _providerFactory.Create(bossEnemy, killEnemyCounter, enemyBossView);
            _enemyHealthViewFactory.Create(bossEnemy.EnemyHealth, enemyBossView.EnemyHealthView);
            
            enemyBossView.FsmOwner.StartBehaviour();

            return enemyBossView;
        }
    }
}