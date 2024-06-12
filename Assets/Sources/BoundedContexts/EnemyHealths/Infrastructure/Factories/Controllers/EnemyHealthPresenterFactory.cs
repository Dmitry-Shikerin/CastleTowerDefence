using Sources.BoundedContexts.Enemies.Controllers;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.EnemyHealths.Controllers;
using Sources.BoundedContexts.EnemyHealths.Domain;
using Sources.BoundedContexts.EnemyHealths.Presentation.Interfaces;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Controllers
{
    public class EnemyHealthPresenterFactory
    {
        public EnemyHealthPresenter Create(EnemyHealth enemyHealth, IEnemyHealthView enemyHealthView) =>
            new(enemyHealth, enemyHealthView);
    }
}