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
        private ICharacterRangeView _view;
        private ICharacterRangeAnimation _animation;
        private CharacterRange _characterRange;

        protected override void OnInit()
        {
            CharacterRangeDependencyProvider provider =
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;

            _view = provider.View;
            _characterRange = provider.CharacterRange;

            _animation = provider.Animation;
        }

        protected override void OnEnter()
        {
            _animation.PlayIdle();
            _characterRange.IsInitialized = true;
            _view.CharacterSpawnPoint.SetCharacterHealth(_view.CharacterHealth);
        }
    }
}