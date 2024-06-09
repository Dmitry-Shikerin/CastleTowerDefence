using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossDeathState : FSMState
    {
        private BossEnemy _enemy;
        private IEnemyBossView _view;
        private IEnemyBossAnimation _animation;
        private IExplosionBodyBloodySpawnService _explosionBodyBloodySpawnService;

        protected override void OnInit()
        {
            EnemyBossDependencyProvider provider =
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;

            _enemy = provider.BossEnemy;
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