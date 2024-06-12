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
        private CharacterMeleeDependencyProvider _provider;
        
        private ICharacterMeleeView View => _provider.View;
        private ICharacterMeleeAnimation Animation => _provider.Animation;
        private ICharacterRotationService RotationService => _provider.CharacterRotationService;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<CharacterMeleeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            Animation.Attacking += OnAttack;
            Animation.PlayAttack();
        }

        protected override void OnUpdate()
        {
            if (View.EnemyHealth == null)
                return;

            if (View.EnemyHealth.CurrentHealth <= 0)
                View.SetEnemyHealth(null);
            
            ChangeLookDirection();
        }

        protected override void OnExit()
        {
            Animation.Attacking -= OnAttack;
        }

        private void OnAttack()
        {
            if (View.EnemyHealth == null)
                return;

            if (View.EnemyHealth.CurrentHealth <= 0)
            {
                View.SetEnemyHealth(null);
                
                return;
            }
            
            View.EnemyHealth.TakeDamage(10);
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