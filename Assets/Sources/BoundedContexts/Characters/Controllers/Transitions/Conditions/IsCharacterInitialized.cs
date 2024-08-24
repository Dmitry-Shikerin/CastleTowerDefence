using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Characters.Controllers.Transitions.Conditions
{
    [Category("Custom/Character")]
    public class IsCharacterInitialized : ConditionTask
    {
        private Character _character;

        [Construct]
        private void Construct(Character character) =>
            _character = character ?? throw new ArgumentNullException(nameof(character));

        protected override bool OnCheck() =>
            _character.IsInitialized;
    }
}