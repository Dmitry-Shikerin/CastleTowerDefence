using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using Sources.Frameworks.Utils.Reflections.Attributes;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyAttackState : FSMState
    {
        private EnemyAttacker _enemyAttacker;
        private IEnemyView _view;
        private IEnemyAnimation _animation;

        [Construct]
        private void Construct(Enemy enemy, IEnemyView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _animation = _view.Animation;
            _enemyAttacker = enemy.EnemyAttacker;
        }
        
        protected override void OnEnter()
        {
            _animation.Attacking += OnAttack;
            _view.Move(_view.CharacterHealthView.Position);
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