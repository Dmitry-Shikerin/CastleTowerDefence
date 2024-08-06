using Doozy.Runtime.Reactor.Animators;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;

namespace Sources.BoundedContexts.Bunkers.Presentation.Interfaces
{
    public interface IBunkerUi
    {
        ITextView HealthText { get; }
        UIAnimator HeartAnimator { get; }
    }
}