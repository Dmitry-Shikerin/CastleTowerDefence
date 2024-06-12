using System;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers
{
    public class BunkerPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public BunkerPresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ??
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public BunkerPresenter Create(IBunkerView bunkerView)
        {
            return new BunkerPresenter(_entityRepository, bunkerView);
        }
    }
}