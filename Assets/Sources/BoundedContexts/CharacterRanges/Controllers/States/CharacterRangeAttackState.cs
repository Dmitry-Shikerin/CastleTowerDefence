using System;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.BoundedContexts.Characters.Presentation.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using Zenject;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    public class CharacterRangeAttackState : FSMState
    {
        private ICharacterRangeView _view;
        private ICharacterAnimation _animation;
        private ICharacterRotationService _rotationService;

        [Construct]
        private void Construct(CharacterRangeView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _animation = view.Animation;
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
            ChangeLookDirection();
            
            if(_view.EnemyHealth.CurrentHealth <= 0)
                _view.SetEnemyHealth(null);
        }

        protected override void OnExit()
        {
            _animation.Attacking -= OnAttack;
        }

        private void OnAttack()
        {
            if (_view.EnemyHealth == null)
                return;
            
            _view.PlayShootParticle();
            _view.EnemyHealth.TakeDamage(2);
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