using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossMoveToCharacterState : FSMState
    {
        private EnemyBossDependencyProvider _provider;
        
        private IEnemyBossView View => _provider.View;
        private IEnemyBossAnimation Animation => _provider.Animation;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;
        }

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