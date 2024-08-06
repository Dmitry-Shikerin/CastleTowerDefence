using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyDeathState : FSMState
    {
        [RequiredField] public BBParameter<EnemyDependencyProvider> _provider;
        
        private IEnemyView View => _provider.value.View;
        private IExplosionBodyBloodySpawnService ExplosionBodyBloodySpawnService => 
            _provider.value.ExplosionBodyBloodySpawnService;

        private PlayerWallet _playerWallet => _provider.value.PlayerWallet;
        
        protected override void OnEnter()
        {
            Vector3 spawnPosition = View.Position + Vector3.up;
            ExplosionBodyBloodySpawnService.Spawn(spawnPosition);
            View.Destroy();
            _playerWallet.AddCoins(1);
        }
    }
}