using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class FromInitializedToCharacterMeleeIdleTransition : ConditionTask
    {
        private CharacterMelee _characterMelee;

        [Construct]
        private void Construct(CharacterMelee characterMelee) =>
            _characterMelee = characterMelee ?? throw new ArgumentNullException(nameof(characterMelee));

        protected override bool OnCheck() =>
            _characterMelee.IsInitialized;
    }
}