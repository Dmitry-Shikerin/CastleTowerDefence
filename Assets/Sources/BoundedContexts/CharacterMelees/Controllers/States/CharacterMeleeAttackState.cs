using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeAttackState : FSMState
    {
        private ICharacterMeleeView _meleeView;
        private ICharacterMeleeAnimation _meleeAnimation;

        protected override void OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;

            _meleeView = provider.MeleeView;
            _meleeAnimation = provider.MeleeAnimation;
        }

        protected override void OnEnter()
        {
            _meleeAnimation.PlayAttack();
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