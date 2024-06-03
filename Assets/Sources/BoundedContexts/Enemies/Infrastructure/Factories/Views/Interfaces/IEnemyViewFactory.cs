using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces
{
    public interface IEnemyViewFactory
    {
        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter);
        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter, EnemyView view);
    }
}