using System;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers
{
    public class EnemySpawnerPresenterFactory
    {
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly IEnemyKamikazeSpawnService _enemyKamikazeSpawnService;
        private readonly IEnemyBossSpawnService _enemyBossSpawnService;

        public EnemySpawnerPresenterFactory(
            IEnemySpawnService enemySpawnService,
            IEnemyKamikazeSpawnService enemyKamikazeSpawnService,
            IEnemyBossSpawnService enemyBossSpawnService)
        {
            _enemySpawnService = enemySpawnService ?? throw new ArgumentNullException(nameof(enemySpawnService));
            _enemyKamikazeSpawnService = enemyKamikazeSpawnService ??
                                         throw new ArgumentNullException(nameof(enemyKamikazeSpawnService));
            _enemyBossSpawnService = enemyBossSpawnService ?? 
                                     throw new ArgumentNullException(nameof(enemyBossSpawnService));
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
                _enemySpawnService,
                _enemyKamikazeSpawnService,
                _enemyBossSpawnService);
        }
    }
}