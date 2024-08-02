using System;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;
using UnityEngine;

namespace Sources.BoundedContexts.Upgrades.Controllers
{
    public class UpgradePresenter : PresenterBase
    {
        private readonly Upgrade _upgrade;
        private readonly PlayerWallet _playerWallet;
        private readonly string _upgradeId;
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
            _upgradeId = upgradeId;
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            UpdatePrice();
            _view.UpgradeButton.onClickEvent.AddListener(ApplyUpgrade);
            _upgrade.LevelChanged += UpdatePrice;
        }

        public override void Disable()
        {
            _view.UpgradeButton.onClickEvent.AddListener(ApplyUpgrade);
            _upgrade.LevelChanged -= UpdatePrice;
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
                
                return;
            }
            
            // Debug.Log($"{_upgradeId} - {_upgrade.CurrentLevel}");
            string price = _upgrade.Levels[_upgrade.CurrentLevel].MoneyPerUpgrade.ToString();
            _view.PriseNextUpgrade.SetText(price);
        }
    }
}