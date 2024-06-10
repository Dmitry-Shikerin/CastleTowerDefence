using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Upgrades.Controllers;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;
using UnityEngine;

namespace Sources.BoundedContexts.Upgrades.Presentation.Implementation
{
    public class UpgradeView : PresentableView<UpgradePresenter>, IUpgradeView
    {
        [Required] [SerializeField] private UIButton _upgradeButton;
        [Required] [SerializeField] private TextView _priceNextUpgrade;
        
        public UIButton UpgradeButton => _upgradeButton;
        public ITextView PriseNextUpgrade => _priceNextUpgrade;
    }
}