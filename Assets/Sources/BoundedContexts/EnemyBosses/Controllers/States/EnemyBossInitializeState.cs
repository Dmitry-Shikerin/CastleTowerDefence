using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyBossInitializeState : FSMState
    {
        private IEnemyBossAnimation _animation;
        private BossEnemy _enemy;

        [Construct]
        private void Construct(BossEnemy enemy, EnemyBossView view)
        {
            _enemy = enemy;
            _animation = view.Animation;
        }

        protected override void OnEnter()
        {
            _enemy.IsInitialized = true;
            _animation.PlayIdle();
        }
    }
}