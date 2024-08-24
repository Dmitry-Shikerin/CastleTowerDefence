using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterRangeInitializeState : FSMState
    {
        private ICharacterRangeView _view;
        private ICharacterAnimation _animation;
        private CharacterRange _characterRange;

        [Construct]
        private void Construct(CharacterRange characterRange, CharacterRangeView characterRangeView)
        {
            _characterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            _view = characterRangeView ?? throw new ArgumentNullException(nameof(characterRangeView));
            _animation = characterRangeView.Animation;
        }

        protected override void OnEnter()
        {
            _animation.PlayIdle();
            _characterRange.IsInitialized = true;
            _view.CharacterSpawnPoint.SetCharacterHealth(_view.CharacterHealth);
        }
    }
}