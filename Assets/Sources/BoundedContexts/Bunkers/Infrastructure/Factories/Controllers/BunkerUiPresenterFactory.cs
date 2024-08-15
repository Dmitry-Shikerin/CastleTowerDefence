using System;
using MyAudios.MyUiFramework.Utils.Soundies.Infrastructure;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers
{
    public class BunkerUiPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public BunkerUiPresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public BunkerUiPresenter Create(BunkerUi view)
        {
            return new BunkerUiPresenter(_entityRepository, view);
        }
    }
}