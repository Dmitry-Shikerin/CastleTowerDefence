using JetBrains.Annotations;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Extensions;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;

namespace Sources.BoundedContexts.Enemies.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToCharacterTransition : ConditionTask
    {
        private IEnemyView _view;

        protected override string OnInit()
        {
            EnemyDependencyProvider provider = blackboard.GetDependencyProvider<EnemyDependencyProvider>();
            _view = provider.View;
            return null;
        }

        protected override bool OnCheck() =>
            _view.CharacterHealthView != null;
    }
}