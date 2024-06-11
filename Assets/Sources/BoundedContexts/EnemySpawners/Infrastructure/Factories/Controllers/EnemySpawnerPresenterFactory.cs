using System;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers
{
    public class EnemySpawnerPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly IEnemyKamikazeSpawnService _enemyKamikazeSpawnService;
        private readonly IEnemyBossSpawnService _enemyBossSpawnService;

        public EnemySpawnerPresenterFactory(
            IEntityRepository entityRepository,
            IEnemySpawnService enemySpawnService,
            IEnemyKamikazeSpawnService enemyKamikazeSpawnService,
            IEnemyBossSpawnService enemyBossSpawnService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _enemySpawnService = enemySpawnService ?? 
                                 throw new ArgumentNullException(nameof(enemySpawnService));
            _enemyKamikazeSpawnService = enemyKamikazeSpawnService ??
                                         throw new ArgumentNullException(nameof(enemyKamikazeSpawnService));
            _enemyBossSpawnService = enemyBossSpawnService ?? 
                                     throw new ArgumentNullException(nameof(enemyBossSpawnService));
        }

        public EnemySpawnerPresenter Create(IEnemySpawnerView view)
        {
            return new EnemySpawnerPresenter(
                _entityRepository,
                view,
                _enemySpawnService,
                _enemyKamikazeSpawnService,
                _enemyBossSpawnService);
        }
    }
}