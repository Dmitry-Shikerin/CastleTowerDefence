using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager.Components;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces
{
    public interface IEnemySpawnerUi
    {
        ITextView CurrentWaveText { get; }
        UISlider SpawnerProgressSlider { get; }
        UIAnimator PopUpAnimator { get; }
    }
}