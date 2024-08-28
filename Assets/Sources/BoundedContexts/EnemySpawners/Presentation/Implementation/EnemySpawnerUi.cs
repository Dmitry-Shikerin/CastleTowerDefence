using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Texts;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Presentation.Implementation
{
    public class EnemySpawnerUi : PresentableView<EnemySpawnerUiPresenter>, IEnemySpawnerUi
    {
        [Required] [SerializeField] private TextView _currentWaveText;
        [Required] [SerializeField] private UISlider _spawnerProgressSlider;
        [Required] [SerializeField] private UIAnimator _popUpAnimator;

        public ITextView CurrentWaveText => _currentWaveText;
        public UISlider SpawnerProgressSlider => _spawnerProgressSlider;
        public UIAnimator PopUpAnimator => _popUpAnimator;
    }
}