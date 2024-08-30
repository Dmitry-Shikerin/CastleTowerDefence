using System;
using System.Collections.Generic;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Characters.Controllers.States;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using Sources.BoundedContexts.EnemyHealths.Presentation.Implementation;
using Sources.BoundedContexts.Layers.Domain;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterMeleeAttackState : CharacterAttackState
    {
        private CharacterView _view;
        private IOverlapService _overlapService;

        [Construct]
        private void Construct(CharacterView view) =>
            _view = view ?? throw new ArgumentNullException(nameof(view));

        [Inject]
        private void Construct(IOverlapService overlapService) =>
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));

        protected override void OnAfterAttack()
        {
            IReadOnlyList<EnemyHealthView> characterHealthViews =
                _overlapService.OverlapSphere<EnemyHealthView>(
                    _view.Position, _view.FindRange,
                    LayerConst.Enemy,
                    LayerConst.Defaul);
            
            if (characterHealthViews.Count == 0)
                return;

            foreach (EnemyHealthView characterHealthView in characterHealthViews)
                characterHealthView.TakeDamage(7);
        }
    }
}