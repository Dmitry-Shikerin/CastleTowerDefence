using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Interfaces
{
    public interface IEnemyKamikazeViewFactory
    {
        IEnemyKamikazeView Create(EnemyKamikaze enemy, KillEnemyCounter killEnemyCounter);
        IEnemyKamikazeView Create(EnemyKamikaze enemy, KillEnemyCounter killEnemyCounter, EnemyKamikazeView view);
    }
}