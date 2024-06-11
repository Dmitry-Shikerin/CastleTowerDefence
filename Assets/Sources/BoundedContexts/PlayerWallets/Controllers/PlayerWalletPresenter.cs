using System;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.PlayerWallets.Controllers
{
    public class PlayerWalletPresenter : PresenterBase
    {
        private readonly PlayerWallet _playerWallet;
        private readonly IPlayerWalletView _view;

        public PlayerWalletPresenter(PlayerWallet playerWallet, IPlayerWalletView view)
        {
            _playerWallet = playerWallet ?? throw new ArgumentNullException(nameof(playerWallet));
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