using System;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views
{
    public class EnemySpawnerViewFactory
    {
        private readonly EnemySpawnerPresenterFactory _presenterFactory;

        public EnemySpawnerViewFactory(EnemySpawnerPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }
        
        public IEnemySpawnerView Create(EnemySpawnerView view)
        {
            EnemySpawnerPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);
            
            return view;
        }
    }
}