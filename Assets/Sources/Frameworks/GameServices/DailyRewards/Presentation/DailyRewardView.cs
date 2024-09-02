using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.DailyRewards.Controllers;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Images;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Images;
using UnityEngine;

namespace Sources.Frameworks.GameServices.DailyRewards.Presentation
{
    public class DailyRewardView : PresentableView<DailyRewardPresenter>
    {
        [Required] [SerializeField] private TextView _timerText;
        [Required] [SerializeField] private UIButton _button;
        [Required] [SerializeField] private ImageView _lockImage;
        [Required] [SerializeField] private SpriteAnimator _animator;
        [Required] [SerializeField] private CanvasGroup _timerView;
        
        public TextView TimerText => _timerText;
        public UIButton Button => _button;
        public IImageView LockImage => _lockImage;
        public SpriteAnimator Animator => _animator;
        public CanvasGroup TimerView => _timerView;
    }
}