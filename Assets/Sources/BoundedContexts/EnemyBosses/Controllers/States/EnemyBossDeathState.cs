using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyBossDeathState : FSMState
    {
        private EnemyBossDependencyProvider _provider;
        
        private IEnemyBossView View => _provider.View;
        private IEnemyBossAnimation Animation => _provider.Animation;
        private IExplosionBodyBloodySpawnService ExplosionBodyBloodySpawnService => 
            _provider.ExplosionBodyBloodySpawnService;
        private PlayerWallet _playerWallet;

        protected override void OnInit()
        {
            _provider =
                graphBlackboard.parent.GetVariable<EnemyBossDependencyProvider>("_provider").value;
            _playerWallet = _provider.PlayerWallet;
        }

        protected override void OnEnter()
        {
            Vector3 spawnPosition = View.Position + Vector3.up;
            ExplosionBodyBloodySpawnService.Spawn(spawnPosition);
            View.Destroy();
            _playerWallet.AddCoins(1);
        }
    }
}