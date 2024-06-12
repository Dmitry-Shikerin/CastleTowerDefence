using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeInitializeState : FSMState
    {
        private CharacterRangeDependencyProvider _provider;
        
        private ICharacterRangeView View => _provider.View;
        private ICharacterRangeAnimation Animation => _provider.Animation;
        private CharacterRange CharacterRange => _provider.CharacterRange;

        protected override void OnInit()
        {
            _provider =
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            Animation.PlayIdle();
            CharacterRange.IsInitialized = true;
            View.CharacterSpawnPoint.SetCharacterHealth(View.CharacterHealth);
        }
    }
}