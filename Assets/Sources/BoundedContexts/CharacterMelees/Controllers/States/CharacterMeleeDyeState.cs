using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterMeleeDyeState : FSMState
    {
        private ICharacterMeleeView _view;

        [Construct]
        private void Construct(CharacterMeleeView characterMeleeView)
        {
            _view = characterMeleeView ?? throw new ArgumentNullException(nameof(characterMeleeView));
        }

        protected override void OnEnter()
        {
            _view.Destroy();
        }
    }
}