using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeAttackState : FSMState
    {
        private ICharacterRangeView _meleeView;
        private ICharacterRangeAnimation _animation;

        protected override void OnInit()
        {
            CharacterRangeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;

            _meleeView = provider.View;
            _animation = provider.Animation;
        }

        protected override void OnEnter()
        {
            _animation.PlayAttack();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}