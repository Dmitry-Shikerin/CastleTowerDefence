using System;
using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class CharacterRangeAttackTransition : ConditionTask
    {
        private ICharacterRangeView _view;

        [Construct]
        private void Construct(CharacterRangeView view) =>
            _view = view ?? throw new ArgumentNullException(nameof(view));

        protected override bool OnCheck() =>
            _view.EnemyHealth != null && Vector3.Distance(
                _view.Position, _view.EnemyHealth.Position) < _view.FindRange;
    }
}