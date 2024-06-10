using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Abilities.Controllers;
using Sources.BoundedContexts.Abilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.UI.Images;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.UI.Images;
using UnityEngine;

namespace Sources.BoundedContexts.Abilities.Presentation.Implementation
{
    public class AbilityApplierView : PresentableView<AbilityApplierPresenter>, IAbilityApplierView
    {
        [Required] [SerializeField] private UIButton _abilityButton;
        [Required] [SerializeField] private ImageView _timerImage;
        
        public UIButton AbilityButton => _abilityButton;
        public IImageView TimerImage => _timerImage;
    }
}