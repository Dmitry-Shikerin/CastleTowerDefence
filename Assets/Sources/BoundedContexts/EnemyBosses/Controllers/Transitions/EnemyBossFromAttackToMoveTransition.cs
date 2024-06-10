using JetBrains.Annotations;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.Transitions
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossFromAttackToMoveTransition : ConditionTask
    {
        private EnemyBossDependencyProvider _provider;
        
        private IEnemyBossView View => _provider.View;
        private BossEnemy Enemy => _provider.BossEnemy;

        protected override string OnInit()
        {
            EnemyBossDependencyProvider provider =
                blackboard.GetVariable<EnemyBossDependencyProvider>("_provider").value;

            return null;
        }

        protected override bool OnCheck() =>
            View.CharacterHealthView == null;
    }
}