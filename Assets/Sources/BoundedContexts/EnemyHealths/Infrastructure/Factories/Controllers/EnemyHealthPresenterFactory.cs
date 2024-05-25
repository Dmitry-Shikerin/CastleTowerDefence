using Sources.BoundedContexts.Enemies.Controllers;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Controllers
{
    public class EnemyHealthPresenterFactory
    {
        public EnemyHealthPresenter Create(EnemyHealth enemyHealth, IEnemyHealthView enemyHealthView) =>
            new(enemyHealth, enemyHealthView);
    }
}