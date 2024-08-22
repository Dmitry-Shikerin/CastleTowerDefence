using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class ToCharacterMeleeDyeTransition : ConditionTask
    {
        private ICharacterMeleeView _view;
        private CharacterMelee _characterMelee;

        [Construct]
        private void Construct(CharacterMelee characterMelee, CharacterMeleeView view)
        {
            _characterMelee = characterMelee ?? throw new ArgumentNullException(nameof(characterMelee));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        protected override bool OnCheck() =>
            _characterMelee.CharacterHealth.CurrentHealth <= 0;
    }
}