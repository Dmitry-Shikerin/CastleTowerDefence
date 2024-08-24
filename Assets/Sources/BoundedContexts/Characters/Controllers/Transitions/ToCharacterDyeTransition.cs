using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Characters.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class ToCharacterDyeTransition : ConditionTask
    {
        private ICharacterView _view;
        private Character _characterMelee;

        [Construct]
        private void Construct(Character characterMelee) =>
            _characterMelee = characterMelee ?? throw new ArgumentNullException(nameof(characterMelee));

        protected override bool OnCheck() =>
            _characterMelee.CharacterHealth.CurrentHealth <= 0;
    }
}