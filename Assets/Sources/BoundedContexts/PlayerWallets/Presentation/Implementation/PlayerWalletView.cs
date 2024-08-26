using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
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
        [Required] [SerializeField] private List<TextView> _moneyTexts;
        [Required] [SerializeField] private UIAnimator _scullAnimator;

        public IReadOnlyList<ITextView> MoneyTexts => _moneyTexts;
        public UIAnimator ScullAnimator => _scullAnimator;
    }
}