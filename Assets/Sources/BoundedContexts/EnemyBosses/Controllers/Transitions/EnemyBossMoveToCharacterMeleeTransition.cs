using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyBossMoveToCharacterMeleeTransition : ConditionTask
    {
        private BossEnemy _enemy;

        [Construct]
        private void Construct(BossEnemy bossEnemy) =>
            _enemy = bossEnemy;

        protected override bool OnCheck() =>
            _enemy.IsInitialized;
    }
}