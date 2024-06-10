using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossInitializeState : FSMState
    {
        private EnemyBossDependencyProvider _provider;
        
        private IEnemyBossAnimation Animation => _provider.Animation;
        private BossEnemy Enemy => _provider.BossEnemy;

        protected override void OnInit()
        {
            _provider = graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            Enemy.IsInitialized = true;
            Animation.PlayIdle();
        }
    }
}