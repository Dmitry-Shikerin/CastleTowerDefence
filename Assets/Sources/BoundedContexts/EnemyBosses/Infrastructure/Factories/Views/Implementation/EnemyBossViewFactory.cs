using System;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Prefabs;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation
{
    public class EnemyBossViewFactory : PoolableObjectFactory<EnemyBossView>,IEnemyBossViewFactory
    {
        private readonly EnemyBossDependencyProviderFactory _providerFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly BurnAbilityViewFactory _burnAbilityViewFactory;

        public EnemyBossViewFactory(
            IObjectPool<EnemyBossView> bossEnemyPool,
            EnemyBossDependencyProviderFactory providerFactory,
            EnemyHealthViewFactory enemyHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            BurnAbilityViewFactory burnAbilityViewFactory) 
            : base(bossEnemyPool)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _burnAbilityViewFactory = burnAbilityViewFactory ?? 
                                      throw new ArgumentNullException(nameof(burnAbilityViewFactory));
        }

        public IEnemyBossView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter, PlayerWallet playerWallet)
        {
            EnemyBossView view = CreateView(PrefabPath.BossEnemy);
            
            return Create(bossEnemy, killEnemyCounter, playerWallet, view);
        }
        
        public IEnemyBossView Create(
            BossEnemy bossEnemy, 
            KillEnemyCounter killEnemyCounter, 
            PlayerWallet playerWallet,
            EnemyBossView view)
        {
            _providerFactory.Create(bossEnemy, killEnemyCounter, playerWallet, view);
            _enemyHealthViewFactory.Create(bossEnemy.EnemyHealth, view.EnemyHealthView);            
            _healthBarViewFactory.Create(bossEnemy.EnemyHealth, view.HealthBarView);
            _burnAbilityViewFactory.Create(bossEnemy.BurnAbility, view.BurnAbilityView);
            
            view.StartFsm();

            return view;
        }
    }
}