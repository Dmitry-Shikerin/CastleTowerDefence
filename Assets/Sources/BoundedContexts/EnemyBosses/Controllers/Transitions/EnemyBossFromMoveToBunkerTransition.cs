using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossFromMoveToBunkerTransition : ConditionTask
    {
        private EnemyBossDependencyProvider _provider;

        private IEnemyBossView View => _provider.View;

        protected override string OnInit()
        {
            _provider =
                blackboard.GetVariable<EnemyBossDependencyProvider>("_provider").value;

            return null;
        }

        protected override bool OnCheck() =>
            View.CharacterHealthView == null;
    }
}