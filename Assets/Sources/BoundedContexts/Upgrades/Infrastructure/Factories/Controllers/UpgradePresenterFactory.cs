using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Upgrades.Controllers;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;

namespace Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Controllers
{
    public class UpgradePresenterFactory
    {
        public UpgradePresenter Create(Upgrade upgrade, PlayerWallet playerWallet, IUpgradeView view)
        {
            return new UpgradePresenter(upgrade, playerWallet, view);
        }
    }
}