using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyMoveToCharacterState : FSMState
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> _provider;
        
        private IEnemyView View => _provider.value.View;
        private IEnemyAnimation Animation => _provider.value.Animation;
        
        protected override void OnEnter() =>
            Animation.PlayWalk();

        protected override void OnUpdate()
        {
            if (View.CharacterHealthView == null)
                return;
            
            if (View.CharacterHealthView.CurrentHealth <= 0)
            {
                View.SetCharacterHealth(null);
                
                return;
            }
            
            View.Move(View.CharacterHealthView.Position);
        }
    }
}