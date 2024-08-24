using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Characters.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterDyeState : FSMState
    {
        private ICharacterView _view;

        [Construct]
        private void Construct(ICharacterView characterMeleeView) =>
            _view = characterMeleeView ?? throw new ArgumentNullException(nameof(characterMeleeView));

        protected override void OnEnter() =>
            _view.Destroy();
    }
}