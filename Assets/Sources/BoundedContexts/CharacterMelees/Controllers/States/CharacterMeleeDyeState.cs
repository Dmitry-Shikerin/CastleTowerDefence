using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeDyeState : FSMState
    {
        private ICharacterMeleeView _view;
        private ICharacterMeleeAnimation _animation;
        private ICharacterRotationService _rotationService;

        protected override void OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;

            _view = provider.View;
            _animation = provider.Animation;
            _rotationService = provider.CharacterRotationService;
        }

        protected override void OnEnter()
        {
            _view.Destroy();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}