using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.CharacterMelees.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterMeleeAttackState : FSMState
    {
        private CharacterMeleeView _view;
        private ICharacterMeleeAnimation _animation;
        private ICharacterRotationService _rotationService;

        [Construct]
        private void Construct(CharacterMeleeView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _animation = _view.MeleeAnimation;
        }

        [Inject]
        private void Construct(ICharacterRotationService rotationService)
        {
            _rotationService = rotationService ?? throw new ArgumentNullException(nameof(rotationService));
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
            {
                _view.SetEnemyHealth(null);
                
                return;
            }
            
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