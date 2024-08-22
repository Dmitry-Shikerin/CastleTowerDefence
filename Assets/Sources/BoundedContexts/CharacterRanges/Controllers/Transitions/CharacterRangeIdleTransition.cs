using System;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.Transitions
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeIdleTransition : ConditionTask
    {
        private ICharacterRangeView _view;
        private CharacterRange _characterRange;

        [Construct]
        private void Construct(CharacterRange characterRange, CharacterRangeView characterRangeView)
        {
            _characterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            _view = characterRangeView ?? throw new ArgumentNullException(nameof(characterRangeView));
        }

        protected override bool OnCheck() =>
            _view.EnemyHealth == null || Vector3.Distance(
                _view.Position, _view.EnemyHealth.Position) > _view.FindRange;
    }
}