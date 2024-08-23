using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterRangeDyeState : FSMState
    {
        private ICharacterRangeView _view;

        [Construct]
        private void Construct(CharacterRangeView view) =>
            _view = view ?? throw new ArgumentNullException(nameof(view));

        protected override void OnEnter() =>
            _view.Destroy();
    }
}