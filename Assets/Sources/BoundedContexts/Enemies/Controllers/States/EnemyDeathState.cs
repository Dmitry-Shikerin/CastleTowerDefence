using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.Enemies.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyDeathState : FSMState
    {
        private IEnemyView _view;
        private ExplosionBodyBloodyViewFactory _explosionBodyBloodyViewFactory;
        private PlayerWallet _playerWallet;
        private KillEnemyCounter _killEnemyCounter;

        [Construct]
        private void Construct(EnemyView view) =>
            _view = view;

        [Inject]
        private void Construct(
            ExplosionBodyBloodyViewFactory explosionBodyViewFactory, 
            IEntityRepository entityRepository)
        {
            _explosionBodyBloodyViewFactory = explosionBodyViewFactory;
            _playerWallet = entityRepository
                .Get<PlayerWallet>(ModelId.PlayerWallet);
            _killEnemyCounter = entityRepository
                .Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
        }
        
        protected override void OnEnter()
        {
            _killEnemyCounter.IncreaseKillCount();
            Vector3 spawnPosition = _view.Position + Vector3.up;
            _explosionBodyBloodyViewFactory.Create(spawnPosition);
            _view.Destroy();
            _playerWallet.AddCoins(1);
        }
    }
}