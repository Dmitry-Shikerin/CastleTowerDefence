using System;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers
{
    public class EnemySpawnerPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly EnemyViewFactory _enemyViewFactory;
        private readonly EnemyKamikazeViewFactory _enemyKamikazeViewFactory;
        private readonly EnemyBossViewFactory _enemyBossViewFactory;
        private readonly ITutorialService _tutorialService;

        public EnemySpawnerPresenterFactory(
            IEntityRepository entityRepository,
            EnemyViewFactory enemyViewFactory,
            EnemyKamikazeViewFactory enemyKamikazeViewFactory,
            EnemyBossViewFactory enemyBossViewFactory,
            ITutorialService tutorialService)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
            _enemyKamikazeViewFactory = enemyKamikazeViewFactory ?? throw new ArgumentNullException(nameof(enemyKamikazeViewFactory));
            _enemyBossViewFactory = enemyBossViewFactory ?? throw new ArgumentNullException(nameof(enemyBossViewFactory));
            _tutorialService = tutorialService ?? throw new ArgumentNullException(nameof(tutorialService));
        }

        public EnemySpawnerPresenter Create(IEnemySpawnerView view)
        {
            return new EnemySpawnerPresenter(
                _entityRepository,
                view,
                _enemyViewFactory,
                _enemyKamikazeViewFactory,
                _enemyBossViewFactory);
        }
    }
}