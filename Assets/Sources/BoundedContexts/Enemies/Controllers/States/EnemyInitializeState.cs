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
        
        private Enemy Enemy => _provider.value.Enemy;
        private IEnemyView View => _provider.value.View;
        private IEnemyAnimation Animation => _provider.value.Animation;
        
        protected override void OnEnter()
        {
            Enemy.IsInitialized = true;
            Animation.PlayIdle();
        }
    }
}