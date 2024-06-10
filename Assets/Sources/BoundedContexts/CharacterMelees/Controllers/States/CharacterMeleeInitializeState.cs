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
        private CharacterMeleeDependencyProvider _provider;
        
        private ICharacterMeleeView View => _provider.View;
        private ICharacterMeleeAnimation Animation => _provider.Animation;
        private CharacterMelee CharacterMelee => _provider.CharacterMelee;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            Animation.PlayIdle();
            CharacterMelee.IsInitialized = true;
            View.CharacterSpawnPoint.SetCharacterHealth(View.HealthView);
        }
    }
}