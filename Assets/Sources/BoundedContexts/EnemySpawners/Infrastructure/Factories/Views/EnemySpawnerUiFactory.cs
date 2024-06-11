using System;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views
{
    public class EnemySpawnerUiFactory
    {
        private readonly EnemySpawnerUiPresenterFactory _presenterFactory;

        public EnemySpawnerUiFactory(EnemySpawnerUiPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? 
                                throw new ArgumentNullException(nameof(presenterFactory));
        }
        
        public IEnemySpawnerUi Create(EnemySpawnerUi view)
        {
            EnemySpawnerUiPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);
            
            return view;
        }
    }
}