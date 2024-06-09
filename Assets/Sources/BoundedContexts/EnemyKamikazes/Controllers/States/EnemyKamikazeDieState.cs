using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeDieState : FSMState
    {
        private EnemyKamikaze _enemy;
        private IEnemyKamikazeView _view;
        private IEnemyAnimation _animation;
        private IExplosionBodyBloodySpawnService _explosionBodyBloodySpawnService;

        protected override void OnInit()
        {
            EnemyKamikazeDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;

            _enemy = provider.EnemyKamikaze;
            _view = provider.View;
            _animation = provider.Animation;
            _explosionBodyBloodySpawnService = provider.ExplosionBodyBloodySpawnService;
        }

        protected override void OnEnter()
        {
            Vector3 spawnPosition = _view.Position + Vector3.up;
            _explosionBodyBloodySpawnService.Spawn(spawnPosition);
            _view.Destroy();
        }
    }
}