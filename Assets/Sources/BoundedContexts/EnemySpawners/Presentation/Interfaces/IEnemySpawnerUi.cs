using Doozy.Runtime.UIManager.Components;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces
{
    public interface IEnemySpawnerUi
    {
        ITextView CurrentWaveText { get; }
        UISlider SpawnerProgressSlider { get; }
    }
}