using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToTargetPointTransition : ConditionTask
    {
        private IEnemyView _view;

        protected override string OnInit()
        {
            EnemyDependencyProvider provider =
                blackboard.GetVariable<EnemyDependencyProvider>("_provider").value;
            _view = provider.View;
            return null;
        }

        protected override bool OnCheck() =>
            _view.CharacterHealthView == null && _view.TargetPoint != null;
    }
}