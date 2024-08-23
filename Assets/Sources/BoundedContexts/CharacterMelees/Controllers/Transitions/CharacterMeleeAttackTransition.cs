using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class CharacterMeleeAttackTransition : ConditionTask
    {
        private ICharacterMeleeView _view;

        [Construct]
        private void Construct(CharacterMeleeView view) =>
            _view = view ?? throw new ArgumentNullException(nameof(view));

        protected override bool OnCheck() =>
            _view.EnemyHealth != null && Vector3.Distance(
                _view.Position, _view.EnemyHealth.Position) < _view.FindRange;
    }
}