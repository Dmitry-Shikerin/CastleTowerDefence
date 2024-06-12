using System;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers
{
    public class EnemySpawnerUiPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public EnemySpawnerUiPresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public EnemySpawnerUiPresenter Create(IEnemySpawnerUi view)
        {
            return new EnemySpawnerUiPresenter(_entityRepository, view);
        }
    }
}