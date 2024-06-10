using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyAttackers.Domain;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyAttackState : FSMState
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> _provider;
        
        private Enemy _enemy;
        private EnemyAttacker _enemyAttacker;
        private IEnemyView _view;
        private IEnemyAnimation _animation;

        protected override void OnInit()
        {
            _enemy = _provider.value.Enemy;
            _enemyAttacker = _enemy.EnemyAttacker;
            _view = _provider.value.View;
            _animation = _provider.value.Animation;
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