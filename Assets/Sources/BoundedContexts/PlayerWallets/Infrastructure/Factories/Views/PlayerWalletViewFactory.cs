using System;
using Sources.BoundedContexts.PlayerWallets.Controllers;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.PlayerWallets.Presentation.Implementation;
using Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces;

namespace Sources.BoundedContexts.PlayerWallets.Infrastructure.Factories.Views
{
    public class PlayerWalletViewFactory
    {
        private readonly PlayerWalletPresenterFactory _presenterFactory;

        public PlayerWalletViewFactory(PlayerWalletPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? 
                                throw new ArgumentNullException(nameof(presenterFactory));
        }
        
        public IPlayerWalletView Create(PlayerWalletView view)
        {
            PlayerWalletPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);
            
            return view;
        }
    }
}