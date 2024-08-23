using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyKamikazeToBomberTransition : ConditionTask
    {
        private IEnemyKamikazeView _view;

        [Construct]
        private void Construct(IEnemyKamikazeView view) =>
            _view = view;

        protected override bool OnCheck() =>
            Vector3.Distance(_view.Position, _view.CharacterMeleePoint.Position)
            <= _view.StoppingDistance;

    }
}