using Sirenix.OdinInspector;
using Sources.BoundedContexts.PlayerWallets.Controllers;
using Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;
using UnityEngine;

namespace Sources.BoundedContexts.PlayerWallets.Presentation.Implementation
{
    public class PlayerWalletView : PresentableView<PlayerWalletPresenter>, IPlayerWalletView
    {
        [Required] [SerializeField] private TextView _moneyText;

        public ITextView MoneyText => _moneyText;
    }
}