using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Characters.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterInitializeState : FSMState
    {
        private ICharacterView _view;
        private ICharacterAnimation _animation;
        private Character _characterRange;

        [Construct]
        private void Construct(Character characterRange, CharacterView characterRangeView)
        {
            _characterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            _view = characterRangeView ?? throw new ArgumentNullException(nameof(characterRangeView));
            _animation = characterRangeView.Animation;
        }

        protected override void OnEnter()
        {
            _animation.PlayIdle();
            _characterRange.IsInitialized = true;
        }
    }
}