using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.Abilities.Controllers;
using Sources.BoundedContexts.Abilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.Abilities.Presentation.Implementation
{
    public class AbilityApplierView : PresentableView<AbilityApplierPresenter>, IAbilityApplierView
    {
        [Required] [SerializeField] private UIButton _abilityButton;
        
        public UIButton AbilityButton => _abilityButton;
    }
}