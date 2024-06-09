using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces
{
    public interface IEnemyBossViewFactory
    {
        IEnemyBossView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter);
        IEnemyBossView Create(BossEnemy bossEnemy, KillEnemyCounter killEnemyCounter, 
            EnemyBosses.Presentation.Implementation.EnemyBossView enemyBossView);
    }
}