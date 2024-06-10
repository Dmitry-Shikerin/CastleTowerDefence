using Doozy.Runtime.UIManager.Components;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;

namespace Sources.BoundedContexts.Upgrades.Presentation.Interfaces
{
    public interface IUpgradeView
    {
        UIButton UpgradeButton { get; }
        ITextView PriseNextUpgrade { get; }
    }
}