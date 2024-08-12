using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.DailyRewards.Controllers;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Texts;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.DailyRewards.Presentation
{
    public class DailyRewardView : PresentableView<DailyRewardPresenter>
    {
        [Required] [SerializeField] private TextView _timerText;
        [Required] [SerializeField] private UIButton _button;
        
        public TextView TimerText => _timerText;
        public UIButton Button => _button;
    }
}