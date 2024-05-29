using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;

namespace Sources.BoundedContexts.CharacterRanges.Controllers.States
{
    [Category("Custom/Character")]
    [UsedImplicitly]
    public class CharacterRangeAttackState : FSMState
    {
        private ICharacterRangeView _view;
        private ICharacterRangeAnimation _animation;
        private ICharacterRotationService _rotationService;

        protected override void OnInit()
        {
            CharacterRangeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;

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