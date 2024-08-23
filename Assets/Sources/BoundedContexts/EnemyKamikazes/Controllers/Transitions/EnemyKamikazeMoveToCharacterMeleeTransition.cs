using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyKamikazeMoveToCharacterMeleeTransition : ConditionTask
    {
        private EnemyKamikaze _enemy;

        [Construct]
        private void Construct(EnemyKamikaze enemyKamikaze) =>
            _enemy = enemyKamikaze;

        protected override bool OnCheck() =>
            _enemy.IsInitialized;
    }
}