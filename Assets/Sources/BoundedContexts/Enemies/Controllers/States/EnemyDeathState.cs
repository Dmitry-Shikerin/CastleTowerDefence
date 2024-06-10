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
        [RequiredField] public BBParameter<EnemyDependencyProvider> _provider;
        
        private Enemy _enemy;
        private IEnemyView _view;
        private IEnemyAnimation _animation;
        private IExplosionBodyBloodySpawnService _explosionBodyBloodySpawnService;

        protected override void OnInit()
        {
            _enemy = _provider.value.Enemy;
            _view = _provider.value.View;
            _animation = _provider.value.Animation;
            _explosionBodyBloodySpawnService = _provider.value.ExplosionBodyBloodySpawnService;
        }

        protected override void OnEnter()
        {
            Vector3 spawnPosition = _view.Position + Vector3.up;
            _explosionBodyBloodySpawnService.Spawn(spawnPosition);
            _view.Destroy();
        }
    }
}