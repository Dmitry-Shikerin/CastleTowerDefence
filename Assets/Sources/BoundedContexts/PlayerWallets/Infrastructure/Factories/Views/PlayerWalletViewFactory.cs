using System;
using Sources.BoundedContexts.PlayerWallets.Controllers;
using Sources.BoundedContexts.PlayerWallets.Presentation.Implementation;
using Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.PlayerWallets.Infrastructure.Factories.Views
{
    public class PlayerWalletViewFactory
    {
        private readonly IEntityRepository _entityRepository;

        public PlayerWalletViewFactory(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
        }
        
        public IPlayerWalletView Create(PlayerWalletView view)
        {
            PlayerWalletPresenter presenter = new PlayerWalletPresenter(_entityRepository, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}