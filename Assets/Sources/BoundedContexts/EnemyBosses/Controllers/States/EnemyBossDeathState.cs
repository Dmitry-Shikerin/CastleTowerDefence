using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.Utils.Reflections.Attributes;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.EnemyBosses.Controllers.States
{
    [Category("Custom/Enemy")]
    public class EnemyBossDeathState : FSMState
    {
        private IEnemyBossView _view; 
        private ExplosionBodyBloodyViewFactory _explosionBodyBloodySpawnService;
        private PlayerWallet _playerWallet;
        private KillEnemyCounter _killEnemyCounter;

        [Construct]
        private void Construct(EnemyBossView view) =>
            _view = view;

        [Inject]
        private void Construct(
            ExplosionBodyBloodyViewFactory explosionBodyBloodySpawnService,
            IEntityRepository entityRepository)
        {
            _explosionBodyBloodySpawnService = explosionBodyBloodySpawnService;
            _playerWallet = entityRepository.Get<PlayerWallet>(ModelId.PlayerWallet);
            _killEnemyCounter = entityRepository.Get<KillEnemyCounter>(ModelId.KillEnemyCounter);
        }

        protected override void OnEnter()
        {
            _killEnemyCounter.IncreaseKillCount();
            Vector3 spawnPosition = _view.Position + Vector3.up;
            _explosionBodyBloodySpawnService.Create(spawnPosition);
            _view.Destroy();
            _playerWallet.AddCoins(1);
        }
    }
}