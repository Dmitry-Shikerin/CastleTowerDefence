using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces
{
    public interface IBossEnemyViewFactory
    {
        IBossEnemyView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter);
        IBossEnemyView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter, 
            EnemyBosses.Presentation.Implementation.BossEnemyView bossEnemyView);
    }
}