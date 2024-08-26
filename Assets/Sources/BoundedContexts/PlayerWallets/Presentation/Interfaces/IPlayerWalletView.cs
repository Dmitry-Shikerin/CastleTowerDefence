using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;

namespace Sources.BoundedContexts.PlayerWallets.Presentation.Interfaces
{
    public interface IPlayerWalletView
    {
        IReadOnlyList<ITextView> MoneyTexts { get; }
        UIAnimator ScullAnimator { get; }
    }
}