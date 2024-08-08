using JetBrains.Annotations;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
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
        private ExplosionBodyBloodyViewFactory ExplosionBodyBloodyviewFactory => 
            _provider.value.ExplosionBodyBloodyViewFactory;

        private PlayerWallet _playerWallet => _provider.value.PlayerWallet;
        
        protected override void OnEnter()
        {
            Vector3 spawnPosition = View.Position + Vector3.up;
            ExplosionBodyBloodyviewFactory.Create(spawnPosition);
            View.Destroy();
            _playerWallet.AddCoins(1);
        }
    }
}