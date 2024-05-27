using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Proveders;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossMoveToCharacterState : FSMState
    {
        private BossEnemy _enemy;
        private IBossEnemyView _view;
        private IBossEnemyAnimation _enemyAnimation;

        protected override void OnInit()
        {
            EnemyBossDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;

            _enemy = provider.BossEnemy;
            _view = provider.BossEnemyView;
            _enemyAnimation = provider.BossEnemyAnimation;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
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