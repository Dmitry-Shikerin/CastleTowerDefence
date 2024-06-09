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
        private EnemyKamikaze _enemy;
        private IEnemyKamikazeView _view;
        private IEnemyAnimation _animation;

        protected override void OnInit()
        {
            EnemyKamikazeDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;

            _enemy = provider.EnemyKamikaze;
            _view = provider.View;
            _animation = provider.Animation;
        }

        protected override void OnEnter()
        {
            _animation.PlayWalk();
        }

        protected override void OnUpdate() =>
            _view.Move(_view.CharacterMeleePoint.Position);
    }
}