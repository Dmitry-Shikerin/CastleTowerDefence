using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class FromInitializedToCharacterRangeIdleTransition : ConditionTask
    {
        private Character _character;

        [Construct]
        private void Construct(Character character) =>
            _character = character ?? throw new ArgumentNullException(nameof(character));

        protected override bool OnCheck() =>
            _character.IsInitialized;
    }
}