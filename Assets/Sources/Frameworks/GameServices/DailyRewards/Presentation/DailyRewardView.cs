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
        
        public TextView TimerText => _timerText;
        public UIButton Button => _button;
        public IImageView LockImage => _lockImage;
    }
}