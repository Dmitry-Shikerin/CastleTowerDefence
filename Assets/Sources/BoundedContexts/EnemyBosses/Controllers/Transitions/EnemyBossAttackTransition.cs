using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossAttackTransition : ConditionTask
    {
        private IEnemyBossView _view;

        [Construct]
        private void Construct(EnemyBossView enemyBossView) =>
            _view = enemyBossView;


        protected override bool OnCheck() =>
            _view.CharacterHealthView != null
            && _view.CharacterHealthView.CurrentHealth > 0
            && Vector3.Distance(_view.Position, _view.CharacterHealthView.Position)
            <= _view.StoppingDistance + 0.15f;
    }
}