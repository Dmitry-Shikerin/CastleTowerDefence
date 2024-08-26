using System;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Controllers.States;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterMeleeAttackState : CharacterAttackState
    {
        private CharacterView _view;

        [Construct]
        private void Construct(CharacterView view) =>
            _view = view ?? throw new ArgumentNullException(nameof(view));

        protected override void OnAfterAttack() =>
            _view.EnemyHealth.TakeDamage(10);
    }
}