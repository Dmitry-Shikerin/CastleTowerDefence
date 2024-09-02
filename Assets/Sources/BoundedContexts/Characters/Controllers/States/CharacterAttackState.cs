using System;
using NodeCanvas.StateMachines;
using Sources.BoundedContexts.Characters.Presentation.Implementation;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Services.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.Characters.Controllers.States
{
    public abstract class CharacterAttackState : FSMState
    {
        private CharacterView _view;
        private ICharacterAnimation _animation;
        private ICharacterRotationService _rotationService;
        
        [Construct]
        private void Construct(CharacterView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _animation = _view.Animation;
        }
        
        [Inject]
        private void Construct(ICharacterRotationService rotationService) =>
            _rotationService = rotationService ?? throw new ArgumentNullException(nameof(rotationService));

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
        
        protected override void OnExit() =>
            _animation.Attacking -= OnAttack;

        private void OnAttack()
        {
            if (_view.EnemyHealth == null)
                return;
        
            if (_view.EnemyHealth.CurrentHealth <= 0)
            {
                _view.SetEnemyHealth(null);
                return;
            }

            OnAfterAttack();
        }
        
        protected abstract void OnAfterAttack();
                
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