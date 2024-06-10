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
        
        private Enemy Enemy => _provider.value.Enemy;
        private EnemyAttacker EnemyAttacker => Enemy.EnemyAttacker;
        private IEnemyView View => _provider.value.View;
        private IEnemyAnimation Animation => _provider.value.Animation;
        
        protected override void OnEnter()
        {
            Animation.Attacking += OnAttack;
            Animation.PlayAttack();
        }

        protected override void OnUpdate() =>
            SetCharacterHealth();

        protected override void OnExit()
        {
            Animation.Attacking -= OnAttack;
            View.SetCharacterHealth(null);
        }

        private void OnAttack()
        {
            SetCharacterHealth();

            if (View.CharacterHealthView == null)
                return;

            View.CharacterHealthView.TakeDamage(EnemyAttacker.Damage);
        }

        private void SetCharacterHealth()
        {
            if (View.CharacterHealthView == null)
                return;
            
            if (View.CharacterHealthView.CurrentHealth > 0)
                return;

            View.SetCharacterHealth(null);
        }
    }
}