using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Characters.PresentationInterfaces;

namespace Sources.BoundedContexts.Characters.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterAttackState : FSMState
    {
        private ICharacterView _view;
        private ICharacterAnimation _animation;

        protected override void OnInit()
        {
            CharacterDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterDependencyProvider>("_provider").value;

            _view = provider.View;
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