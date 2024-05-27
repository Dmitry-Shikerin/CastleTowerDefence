using System;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Domain;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation;
using Sources.BoundedContexts.EnemySpawners.Presentationinterfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views
{
    public class EnemySpawnerViewFactory
    {
        private readonly EnemySpawnerPresenterFactory _presenterFactory;

        public EnemySpawnerViewFactory(EnemySpawnerPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }
        
        public IEnemySpawnerView Create(
            EnemySpawner enemySpawner, 
            KillEnemyCounter killEnemyCounter,
            EnemySpawnerView view)
        {
            EnemySpawnerPresenter presenter = _presenterFactory.Create(enemySpawner, killEnemyCounter, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}