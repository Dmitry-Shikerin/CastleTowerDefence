using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers
{
    public class EnemySpawnerUiPresenterFactory
    {
        public EnemySpawnerUiPresenter Create(EnemySpawner enemySpawner, IEnemySpawnerUi view)
        {
            return new EnemySpawnerUiPresenter(enemySpawner, view);
        }
    }
}