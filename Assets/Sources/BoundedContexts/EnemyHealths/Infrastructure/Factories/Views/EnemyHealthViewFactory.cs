using System;
using Sources.BoundedContexts.Enemies.Controllers;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views
{
    public class EnemyHealthViewFactory
    {
        private readonly EnemyHealthPresenterFactory _enemyHealthPresenterFactory;

        public EnemyHealthViewFactory(EnemyHealthPresenterFactory enemyHealthPresenterFactory)
        {
            _enemyHealthPresenterFactory = enemyHealthPresenterFactory ?? 
                                           throw new ArgumentNullException(nameof(enemyHealthPresenterFactory));
        }

        public IEnemyHealthView Create(EnemyHealth enemyHealth, EnemyHealthView enemyHealthView)
        {
            EnemyHealthPresenter enemyHealthPresenter =
                _enemyHealthPresenterFactory.Create(enemyHealth, enemyHealthView);
            
            enemyHealthView.Construct(enemyHealthPresenter);
            
            return enemyHealthView;
        }
    }
}