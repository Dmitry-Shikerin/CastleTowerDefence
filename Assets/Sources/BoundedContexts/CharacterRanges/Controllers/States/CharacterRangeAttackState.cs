using System;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Controllers.States;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterRangeAttackState : CharacterAttackState
    {
        private ICharacterRangeView _view;

        [Construct]
        private void Construct(CharacterRangeView view) =>
            _view = view ?? throw new ArgumentNullException(nameof(view));

        protected override void OnAfterAttack()
        {
            _view.PlayShootParticle();
            _view.EnemyHealth.TakeDamage(2);
        }
    }
}