using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Upgrades.Controllers;
using Sources.BoundedContexts.Upgrades.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Upgrades.Presentation.Implementation
{
    public class UpgradeView : PresentableView<UpgradePresenter>, IUpgradeView
    {
        [Required] [SerializeField] private UIButton _upgradeButton;
        
        public UIButton UpgradeButton => _upgradeButton;
    }
}