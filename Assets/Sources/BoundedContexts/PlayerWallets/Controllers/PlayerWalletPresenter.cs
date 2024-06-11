using System;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.PlayerWallets.Controllers
{
    public class PlayerWalletPresenter : PresenterBase
    {
        private readonly PlayerWallet _playerWallet;
        private readonly IPlayerWalletView _view;

        public PlayerWalletPresenter(IEntityRepository entityRepository, IPlayerWalletView view)
        {
            if (entityRepository == null) 
                throw new ArgumentNullException(nameof(entityRepository));
            
            _playerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            OnCoinsChanged();
            _playerWallet.CoinsChanged += OnCoinsChanged;
        }

        public override void Disable() =>
            _playerWallet.CoinsChanged -= OnCoinsChanged;

        private void OnCoinsChanged() =>
            _view.MoneyText.SetText(_playerWallet.Coins.ToString());
    }
}