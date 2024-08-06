﻿using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    [UsedImplicitly]
    public class EnemyKamikazeDieState : FSMState
    {
        private EnemyKamikazeDependencyProvider _provider;
        private PlayerWallet _playerWallet;
        
        private IEnemyKamikazeView View => _provider.View; 
        private IExplosionBodyBloodySpawnService ExplosionBodyBloodySpawnService => 
            _provider.ExplosionBodyBloodySpawnService;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
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