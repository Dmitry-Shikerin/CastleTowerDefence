using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyAttackers.Domain;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyAttackState : FSMState
    {
        private Enemy _enemy;
        private EnemyAttacker _enemyAttacker;
        private IEnemyView _view;
        private IEnemyAnimation _animation;

        protected override void OnInit()
        {
            EnemyDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyDependencyProvider>("_provider").value;

            _enemy = provider.Enemy;
            _enemyAttacker = _enemy.EnemyAttacker;
            _view = provider.View;
            _animation = provider.Animation;
        }

        protected override void OnEnter()
        {
            _animation.Attacking += OnAttack;
            _animation.PlayAttack();
        }

        protected override void OnUpdate()
        {
            if (_view.CharacterHealthView == null)
                return;
            
            if (_view.CharacterHealthView.CurrentHealth <= 0)
            {
                Debug.Log($"Character died: {_view.CharacterHealthView.CurrentHealth}");
                _view.SetCharacterHealth(null);
            }
        }

        protected override void OnExit()
        {
            _animation.Attacking -= OnAttack;
            _view.SetCharacterHealth(null);
        }

        private void OnAttack()
        {
            if (_view.CharacterHealthView == null)
                return;
            
            _view.CharacterHealthView.TakeDamage(_enemyAttacker.Damage);
        }
    }
}