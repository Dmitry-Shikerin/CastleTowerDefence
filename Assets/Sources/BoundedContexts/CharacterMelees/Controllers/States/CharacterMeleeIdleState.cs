using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.InfrastructureInterfaces.Services.Overlaps;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeIdleState : FSMState
    {
        private ICharacterMeleeView _meleeView;
        private ICharacterMeleeAnimation _meleeAnimation;
        private IOverlapService _overlapService;

        protected override void OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;

            _meleeView = provider.MeleeView;
            _meleeAnimation = provider.MeleeAnimation;
            _overlapService = provider.OverlapService;
        }

        protected override void OnEnter()
        {
            _meleeAnimation.PlayIdle();
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnExit()
        {
        }
        
        private void FindTarget()
        {
            
        }
    }
}