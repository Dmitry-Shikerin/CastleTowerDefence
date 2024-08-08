﻿using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
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
        private ExplosionBodyBloodyViewFactory ExplosionBodyBloodyViewFactory => 
            _provider.ExplosionBodyBloodyViewFactory;

        protected override void OnInit()
        {
            _provider = 
                graphBlackboard.parent.GetVariable<EnemyKamikazeDependencyProvider>("_provider").value;
            _playerWallet = _provider.PlayerWallet;
        }

        protected override void OnEnter()
        {
            Vector3 spawnPosition = View.Position + Vector3.up;
            ExplosionBodyBloodyViewFactory.Create(spawnPosition);
            View.Destroy();
            _playerWallet.AddCoins(1);
        }
    }
}