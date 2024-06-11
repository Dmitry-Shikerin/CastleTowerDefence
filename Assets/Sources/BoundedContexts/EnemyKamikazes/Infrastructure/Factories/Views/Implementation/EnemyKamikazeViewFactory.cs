using System;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.BoundedContexts.Prefabs;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Implementation
{
    public class EnemyKamikazeViewFactory : PoolableObjectFactory<EnemyKamikazeView>, IEnemyKamikazeViewFactory
    {
        private readonly EnemyKamikazeDependencyProviderFactory _dependencyProviderFactory;
        private readonly EnemyHealthViewFactory _enemyHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly BurnAbilityViewFactory _burnAbilityViewFactory;

        public EnemyKamikazeViewFactory(
            EnemyKamikazeDependencyProviderFactory dependencyProviderFactory,
            EnemyHealthViewFactory enemyHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            IObjectPool<EnemyKamikazeView> pool,
            BurnAbilityViewFactory burnAbilityViewFactory) 
            : base(pool)
        {
            _dependencyProviderFactory = dependencyProviderFactory ?? 
                                         throw new ArgumentNullException(nameof(dependencyProviderFactory));
            _enemyHealthViewFactory = enemyHealthViewFactory ?? 
                                      throw new ArgumentNullException(nameof(enemyHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _burnAbilityViewFactory = burnAbilityViewFactory ?? 
                                      throw new ArgumentNullException(nameof(burnAbilityViewFactory));
        }
        
        public IEnemyKamikazeView Create(EnemyKamikaze enemy, KillEnemyCounter killEnemyCounter)
        {
            EnemyKamikazeView enemyView = CreateView(PrefabPath.EnemyKamikaze);
            
            return Create(enemy, killEnemyCounter, enemyView);
        }

        public IEnemyKamikazeView Create(
            EnemyKamikaze enemy, 
            KillEnemyCounter killEnemyCounter, 
            EnemyKamikazeView view)
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