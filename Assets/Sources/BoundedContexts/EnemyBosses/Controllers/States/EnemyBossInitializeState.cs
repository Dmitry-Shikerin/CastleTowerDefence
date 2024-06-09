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
        private IEnemyBossAnimation _animation;
        private BossEnemy _enemy;
        private IEnemyBossView _view;

        protected override void OnInit()
        {
            var provider = graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;
            _enemy = provider.BossEnemy;
            _view = provider.View;
            _animation = provider.Animation;
        }

        protected override void OnEnter()
        {
            _enemy.IsInitialized = true;
            _animation.PlayIdle();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit()
        {
            base.OnExit();
        }
    }
}