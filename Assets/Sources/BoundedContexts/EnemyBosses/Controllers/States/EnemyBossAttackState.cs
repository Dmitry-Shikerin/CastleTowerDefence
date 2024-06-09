using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossAttackState : FSMState
    {
        private BossEnemy _enemy;
        private EnemyAttacker _enemyAttacker;
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;
                
        protected override void OnInit()
        {
            EnemyBossDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;
        
            _enemy = provider.BossEnemy;
            _enemyAttacker = _enemy.EnemyAttacker;
            _view = provider.View;
            _animation = provider.Animation;
        }
        
        protected override void OnEnter()
        {
            _animation.Attacking += OnAttack;
            _animation.PlayAttack();
        }
        
        protected override void OnUpdate() =>
            SetCharacterHealth();
        
        protected override void OnExit()
        {
            _animation.Attacking -= OnAttack;
            _view.SetCharacterHealth(null);
        }
        
        private void OnAttack()
        {
            SetCharacterHealth();
        
            if (_view.CharacterHealthView == null)
                        return;
        
            _view.CharacterHealthView.TakeDamage(_enemyAttacker.Damage);
        }
        
        private void SetCharacterHealth()
        {
            if (_view.CharacterHealthView == null)
                return;
                    
            if (_view.CharacterHealthView.CurrentHealth > 0)
                return;
        
            _view.SetCharacterHealth(null);
        }
    }
}