using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.EnemyKamikazes.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyKamikazeDieState : FSMState
    {
        private PlayerWallet _playerWallet;
        private KillEnemyCounter _killEnemyCounter;
        private IEnemyKamikazeView _view; 
        private ExplosionBodyBloodyViewFactory _explosionBodyBloodyViewFactory;

        [Construct]
        private void Construct(EnemyKamikazeView view) =>
            _view = view;

        [Inject]
        private void Construct(
            ExplosionBodyBloodyViewFactory explosionBodyBloodyViewFactory,
            IEntityRepository entityRepository)
        {
            _explosionBodyBloodyViewFactory = explosionBodyBloodyViewFactory;
            _playerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
            _killEnemyCounter = entityRepository.Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
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