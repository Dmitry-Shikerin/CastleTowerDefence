using System;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views
{
    public class EnemySpawnerUiFactory
    {
        private readonly IEntityRepository _entityRepository;

        public EnemySpawnerUiFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }
        
        public IEnemySpawnerUi Create(EnemySpawnerUi view)
        {
            EnemySpawnerUiPresenter presenter = new EnemySpawnerUiPresenter(_entityRepository, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}