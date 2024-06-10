using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyInitializeState : FSMState
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> _provider;
        
        private Enemy _enemy;
        private IEnemyView _view;
        private IEnemyAnimation _animation;

        protected override void OnInit()
        {
            _enemy = _provider.value.Enemy;
            _view = _provider.value.View;
            _animation = _provider.value.Animation;
        }

        protected override void OnEnter()
        {
            _enemy.IsInitialized = true;
            _animation.PlayIdle();
        }
    }
}