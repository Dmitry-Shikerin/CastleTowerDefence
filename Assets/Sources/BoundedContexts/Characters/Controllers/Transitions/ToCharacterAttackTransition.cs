using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.Characters.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class ToCharacterAttackTransition : ConditionTask
    {
        private ICharacterView _view;

        [Construct]
        private void Construct(CharacterView view) =>
            _view = view ?? throw new ArgumentNullException(nameof(view));

        protected override bool OnCheck() =>
            _view.EnemyHealth != null && Vector3.Distance(
                _view.Position, _view.EnemyHealth.Position) < _view.FindRange;
    }
}