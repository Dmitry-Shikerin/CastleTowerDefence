using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterRangeDyeState : FSMState
    {
        private ICharacterRangeView _view;
        private CharacterRange _characterRange;

        [Construct]
        private void Construct(CharacterRange characterRange, CharacterRangeView view)
        {
            _characterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        protected override void OnEnter()
        {
            _view.Destroy();
        }
    }
}