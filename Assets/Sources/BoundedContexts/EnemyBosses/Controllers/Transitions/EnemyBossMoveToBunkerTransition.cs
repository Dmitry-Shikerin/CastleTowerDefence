using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyBossMoveToBunkerTransition : ConditionTask
    {
        private IEnemyBossView _view;

        [Construct]
        private void Construct(IEnemyBossView view) =>
            _view = view;

        protected override bool OnCheck() =>
            Vector3.Distance(_view.Position, _view.CharacterMeleePoint.Position)
            <= _view.StoppingDistance && _view.CharacterHealthView == null;

    }
}