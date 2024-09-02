using System;
using Doozy.Runtime.UIManager;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.Upgrades.Controllers
{
    public class UpgradePresenter : PresenterBase
    {
        private readonly Upgrade _upgrade;
        private readonly PlayerWallet _playerWallet;
        private readonly IUpgradeView _view;

        public UpgradePresenter(
            IEntityRepository entityRepository,
            string upgradeId, 
            IUpgradeView view)
        {
            if (entityRepository == null) 
                throw new ArgumentNullException(nameof(entityRepository));
            
            _upgrade = entityRepository.Get<Upgrade>(upgradeId);
            _playerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            UpdatePrice();
            _view.UpgradeButton.onClickEvent.AddListener(ApplyUpgrade);
            _upgrade.LevelChanged += UpdatePrice;
            _playerWallet.CoinsChanged += UpdateButton;
        }

        public override void Disable()
        {
            _view.UpgradeButton.onClickEvent.AddListener(ApplyUpgrade);
            _upgrade.LevelChanged -= UpdatePrice;
            _playerWallet.CoinsChanged -= UpdateButton;
        }

        private void ApplyUpgrade()
        {
            _upgrade.ApplyUpgrade(_playerWallet);
        }

        private void UpdatePrice()
        {
            if (_upgrade.CurrentLevel >= _upgrade.MaxLevel)
            {
                _view.PriseNextUpgrade.SetText("Max");
                _view.SkullIcon.Hide();
                _view.UpgradeButton.enabled = false;
                _view.UpgradeButton.SetState(UISelectionState.Disabled);
                
                return;
            }
            
            // Debug.Log($"{_upgradeId} - {_upgrade.CurrentLevel}");
            string price = _upgrade.Levels[_upgrade.CurrentLevel].MoneyPerUpgrade.ToString();
            _view.PriseNextUpgrade.SetText(price);
            UpdateButton();
        }

        private void UpdateButton()
        {
            int price = _upgrade.Levels[_upgrade.CurrentLevel].MoneyPerUpgrade;
            
            if (price > _playerWallet.Coins)
            {
                _view.UpgradeButton.enabled = false;
                _view.UpgradeButton.SetState(UISelectionState.Disabled);
                return;
            }
            
            _view.UpgradeButton.enabled = true;
            _view.UpgradeButton.SetState(UISelectionState.Normal);
        }
    }
}