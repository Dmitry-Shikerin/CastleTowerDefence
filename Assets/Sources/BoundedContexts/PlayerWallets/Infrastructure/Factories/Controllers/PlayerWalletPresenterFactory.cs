using Sources.BoundedContexts.PlayerWallets.Controllers;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces;

namespace Sources.BoundedContexts.PlayerWallets.Infrastructure.Factories.Controllers
{
    public class PlayerWalletPresenterFactory
    {
        public PlayerWalletPresenter Create(PlayerWallet playerWallet, IPlayerWalletView view)
        {
            return new PlayerWalletPresenter(playerWallet, view);
        }
    }
}