using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyDeathState : FSMState
    {
        private Enemy _enemy;
        private IEnemyView _view;
        private IEnemyAnimation _animation;
        private IExplosionBodyBloodySpawnService _explosionBodyBloodySpawnService;

        protected override void OnInit()
        {
            EnemyDependencyProvider provider = 
                graphBlackboard.parent.GetVariable<EnemyDependencyProvider>("_provider").value;

            _enemy = provider.Enemy;
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