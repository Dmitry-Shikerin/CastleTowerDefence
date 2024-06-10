using System;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Upgrades.Controllers;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Upgrades.Presentation.Implementation;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;

namespace Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Views
{
    public class UpgradeViewFactory
    {
        private readonly UpgradePresenterFactory _presenterFactory;

        public UpgradeViewFactory(UpgradePresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IUpgradeView Create(Upgrade upgrade, PlayerWallet playerWallet, UpgradeView view)
        {
            UpgradePresenter presenter = _presenterFactory.Create(upgrade, playerWallet, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}