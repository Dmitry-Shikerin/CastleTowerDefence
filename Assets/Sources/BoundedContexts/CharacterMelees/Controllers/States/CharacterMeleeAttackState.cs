using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterMeleeAttackState : FSMState
    {
        private ICharacterMeleeView _view;
        private ICharacterMeleeAnimation _animation;
        private ICharacterRotationService _rotationService;

        protected override void OnInit()
        {
            CharacterMeleeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;

            _view = provider.View;
            _animation = provider.Animation;
            _rotationService = provider.CharacterRotationService;
        }

        protected override void OnEnter()
        {
            _animation.Attacking += OnAttack;
            _animation.PlayAttack();
        }

        protected override void OnUpdate()
        {
            if (_view.EnemyHealth == null)
                return;

            if (_view.EnemyHealth.CurrentHealth <= 0)
                _view.SetEnemyHealth(null);
            
            ChangeLookDirection();
        }

        protected override void OnExit()
        {
            _animation.Attacking -= OnAttack;
        }

        private void OnAttack()
        {
            if (_view.EnemyHealth == null)
                return;

            if (_view.EnemyHealth.CurrentHealth <= 0)
                _view.SetEnemyHealth(null);
            
            _view.EnemyHealth.TakeDamage(10);
        }
        
        private void ChangeLookDirection()
        {
            if (_view.EnemyHealth == null)
                return;

            float angle = _rotationService.GetAngleRotation(
                _view.EnemyHealth.Position, _view.Position);
            _view.SetLookRotation(angle);
        }
    }
}