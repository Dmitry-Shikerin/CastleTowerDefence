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
        private CharacterRangeDependencyProvider _provider;
        
        private ICharacterRangeView View => _provider.View;
        private ICharacterRangeAnimation Animation => _provider.Animation;
        private ICharacterRotationService RotationService => _provider.CharacterRotationService;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<CharacterRangeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            Animation.Attacking += OnAttack;
            Animation.PlayAttack();
        }

        protected override void OnUpdate()
        {
            ChangeLookDirection();
            
            if(View.EnemyHealth.CurrentHealth <= 0)
                View.SetEnemyHealth(null);
        }

        protected override void OnExit()
        {
            Animation.Attacking -= OnAttack;
        }

        private void OnAttack()
        {
            if (View.EnemyHealth == null)
                return;
            
            View.PlayShootParticle();
            View.EnemyHealth.TakeDamage(2);
        }
        
        private void ChangeLookDirection()
        {
            if (View.EnemyHealth == null)
                return;

            float angle = RotationService.GetAngleRotation(
                View.EnemyHealth.Position, View.Position);
            View.SetLookRotation(angle);
        }
    }
}