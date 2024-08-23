using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    public class EnemyKamikazeToDeathTransition : ConditionTask
    {
        private IEnemyKamikazeView _view;

        [Construct]
        private void Construct(EnemyKamikazeView view) =>
            _view = view;

        protected override bool OnCheck() =>
            _view.EnemyHealthView.CurrentHealth <= 0;
    }
}