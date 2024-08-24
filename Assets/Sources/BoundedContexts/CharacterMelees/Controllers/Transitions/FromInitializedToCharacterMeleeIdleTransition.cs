using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.Characters.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class FromInitializedToCharacterMeleeIdleTransition : ConditionTask
    {
        private Character _character;

        [Construct]
        private void Construct(Character character) =>
            _character = character ?? throw new ArgumentNullException(nameof(character));

        protected override bool OnCheck() =>
            _character.IsInitialized;
    }
}