using Doozy.Runtime.UIManager.Components;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Images;

namespace Sources.BoundedContexts.Abilities.Presentation.Interfaces
{
    public interface IAbilityApplierView
    {
        UIButton AbilityButton { get; }
        IImageView TimerImage { get; }
    }
}