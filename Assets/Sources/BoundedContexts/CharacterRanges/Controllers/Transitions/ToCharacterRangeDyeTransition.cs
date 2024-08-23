using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class ToCharacterRangeDyeTransition : ConditionTask
    {
        private CharacterRange _characterRange;

        [Construct]
        private void Construct(CharacterRange characterRange) =>
            _characterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));

        protected override bool OnCheck() =>
            _characterRange.CharacterHealth.CurrentHealth <= 0;

    }
}