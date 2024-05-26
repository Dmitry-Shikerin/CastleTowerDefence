using Sources.BoundedContexts.BossEnemyView.Domain;
using Sources.BoundedContexts.BossEnemyView.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.BossEnemyView.Infrastructure.Factories.Views.Interfaces
{
    public interface IBossEnemyViewFactory
    {
        IBossEnemyView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter);
        IBossEnemyView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter, Presentation.Implementation.BossEnemyView bossEnemyView);
    }
}