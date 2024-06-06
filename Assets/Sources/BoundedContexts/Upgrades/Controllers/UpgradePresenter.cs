using System;
using Sources.BoundedContexts.Players.Domain;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.Upgrades.Controllers
{
    public class UpgradePresenter : PresenterBase
    {
        private readonly Upgrade _upgrade;
        private readonly PlayerWallet _playerWallet;
        private readonly IUpgradeView _view;

        public UpgradePresenter(
            Upgrade upgrade, 
            PlayerWallet playerWallet, 
            IUpgradeView view)
        {
            _upgrade = upgrade ?? throw new ArgumentNullException(nameof(upgrade));
            _playerWallet = playerWallet ?? throw new ArgumentNullException(nameof(playerWallet));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable() =>
            _view.UpgradeButton.onClickEvent.AddListener(ApplyUpgrade);

        public override void Disable() =>
            _view.UpgradeButton.onClickEvent.AddListener(ApplyUpgrade);

        private void ApplyUpgrade() =>
            _upgrade.ApplyUpgrade(_playerWallet);
    }
}