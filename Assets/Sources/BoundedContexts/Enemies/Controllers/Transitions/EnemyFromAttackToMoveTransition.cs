using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyFromAttackToMoveTransition : ConditionTask
    {
        private IEnemyView _view;
        private Enemy _enemy;

        protected override string OnInit()
        {
            EnemyDependencyProvider provider =
                blackboard.GetVariable<EnemyDependencyProvider>("_provider").value;
            _enemy = provider.Enemy;
            _view = provider.View;

            return null;
        }

        protected override bool OnCheck() =>
            _view.CharacterHealthView == null;
    }
}