using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeMoveToCharacterMeleePointState : FSMState
    {
        private EnemyKamikazeDependencyProvider _provider;
        
        private IEnemyKamikazeView View => _provider.View;
        private IEnemyAnimation Animation => _provider.Animation;

        protected override void OnInit()
        {
            _provider =
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            Animation.PlayWalk();
        }

        protected override void OnUpdate() =>
            View.Move(View.CharacterMeleePoint.Position);
    }
}