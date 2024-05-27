using System;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Domain;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers
{
    public class EnemySpawnerPresenterFactory
    {
        private readonly IEnemySpawnService _enemySpawnService;

        public EnemySpawnerPresenterFactory(
            IEnemySpawnService enemySpawnService)
        {
            _enemySpawnService = enemySpawnService ?? throw new ArgumentNullException(nameof(enemySpawnService));
        }

        public EnemySpawnerPresenter Create(
            EnemySpawner enemySpawner, 
            KillEnemyCounter killEnemyCounter,
            IEnemySpawnerView view)
        {
            return new EnemySpawnerPresenter(
                enemySpawner, 
                killEnemyCounter,
                view,
                _enemySpawnService);
        }
    }
}