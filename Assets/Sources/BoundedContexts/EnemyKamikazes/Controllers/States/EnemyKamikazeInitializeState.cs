using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeInitializeState : FSMState
    {
        private EnemyKamikazeDependencyProvider _provider;
        
        private EnemyKamikaze Enemy => _provider.EnemyKamikaze;
        private IEnemyAnimation Animation => _provider.Animation;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
        }

        protected override void OnEnter()
        {
            Enemy.IsInitialized = true;
            Animation.PlayIdle();
        }
    }
}