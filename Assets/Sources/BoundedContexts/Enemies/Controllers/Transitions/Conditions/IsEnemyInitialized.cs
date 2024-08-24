using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions.Conditions
{
    [Category("Custom/Enemy")]
    public class IsEnemyInitialized : ConditionTask
    {
        private Enemy _enemy;

        [Construct]
        private void Construct(Enemy enemy) =>
            _enemy = enemy;

        protected override bool OnCheck() =>
            _enemy.IsInitialized;
    }
}