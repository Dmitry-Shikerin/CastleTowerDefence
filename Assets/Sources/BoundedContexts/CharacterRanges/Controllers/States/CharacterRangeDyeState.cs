using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeDyeState : FSMState
    {
        private ICharacterRangeView _view;
        private ICharacterRangeAnimation _animation;
        private ICharacterRotationService _rotationService;

        protected override void OnInit()
        {
            CharacterRangeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;

            _view = provider.View;
            _animation = provider.Animation;
            _rotationService = provider.CharacterRotationService;
        }

        protected override void OnEnter()
        {
            _view.Destroy();
        }
    }
}