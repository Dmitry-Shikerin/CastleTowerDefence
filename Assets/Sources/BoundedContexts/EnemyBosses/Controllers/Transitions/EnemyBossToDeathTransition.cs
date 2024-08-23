using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyBossToDeathTransition : ConditionTask
    {
        private IEnemyBossView _view;

        [Construct]
        private void Construct(IEnemyBossView view) =>
            _view = view;

        protected override bool OnCheck() =>
            _view.EnemyHealthView.CurrentHealth <= 0;
    }
}