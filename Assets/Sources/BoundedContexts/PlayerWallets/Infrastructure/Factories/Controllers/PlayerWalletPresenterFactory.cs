using System;
using Sources.BoundedContexts.PlayerWallets.Controllers;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.PlayerWallets.Infrastructure.Factories.Controllers
{
    public class PlayerWalletPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;

        public PlayerWalletPresenterFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }

        public PlayerWalletPresenter Create(IPlayerWalletView view)
        {
            return new PlayerWalletPresenter(_entityRepository, view);
        }
    }
}