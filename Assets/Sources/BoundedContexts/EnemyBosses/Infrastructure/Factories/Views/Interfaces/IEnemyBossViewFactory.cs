using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.KillEnemyCounters.Domain.Models.Implementation;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces
{
    public interface IEnemyBossViewFactory
    {
        IEnemyBossView Create(BossEnemy bossEnemy);
        IEnemyBossView Create(BossEnemy bossEnemy,
            EnemyBosses.Presentation.Implementation.EnemyBossView view);
    }
}