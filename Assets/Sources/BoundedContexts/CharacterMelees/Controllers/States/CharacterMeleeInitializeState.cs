using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeInitializeState : FSMState
    {
        private ICharacterMeleeView _view;
        private ICharacterMeleeAnimation _animation;
        private CharacterMelee _characterMelee;

        protected override void OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;

            _view = provider.View;
            _characterMelee = provider.CharacterMelee;
            
            _animation = provider.Animation;
        }

        protected override void OnEnter()
        {
            _animation.PlayIdle();
            _characterMelee.IsInitialized = true;
            _view.CharacterSpawnPoint.SetCharacterHealth(_view.HealthView);
        }
    }
}