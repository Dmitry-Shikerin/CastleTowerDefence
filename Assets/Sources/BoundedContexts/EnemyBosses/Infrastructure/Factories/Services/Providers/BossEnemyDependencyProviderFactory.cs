using Sources.BoundedContexts.EnemyBosses.Domain;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Proveders;
using Sources.BoundedContexts.EnemyBosses.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;

namespace Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers
{
    public class BossEnemyDependencyProviderFactory
    {
        public EnemyBossDependencyProvider Create(
            BossEnemy bossEnemy, 
            KillEnemyCounter killEnemyCounter, 
            IBossEnemyView bossEnemyView)
        {
            EnemyBossDependencyProvider provider = bossEnemyView.Provider;
            provider.Construct(
                bossEnemy, 
                killEnemyCounter, 
                bossEnemyView, 
                bossEnemyView.Animation);

            return provider;
        }
    }
}