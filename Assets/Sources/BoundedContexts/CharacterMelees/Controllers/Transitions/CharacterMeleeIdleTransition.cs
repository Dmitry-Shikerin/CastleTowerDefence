using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.Transitions
{
    [Category("Custom/Character")]
    public class CharacterMeleeIdleTransition : ConditionTask
    {
        private ICharacterMeleeView _view;
        private CharacterMelee _characterMelee;

        [Construct]
        private void Construct(CharacterMelee characterMelee, CharacterMeleeView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _characterMelee = characterMelee ?? throw new ArgumentNullException(nameof(characterMelee));
        }

        protected override bool OnCheck() =>
            _view.EnemyHealth == null || Vector3.Distance(
                _view.Position, _view.EnemyHealth.Position) > _view.FindRange;
    }
}