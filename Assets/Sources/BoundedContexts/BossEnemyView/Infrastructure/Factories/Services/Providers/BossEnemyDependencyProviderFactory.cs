using Sources.BoundedContexts.BossEnemyView.Domain;
using Sources.BoundedContexts.BossEnemyView.Infrastructure.Services.Proveders;
using Sources.BoundedContexts.BossEnemyView.Presentation.Interfaces;

namespace Sources.BoundedContexts.BossEnemyView.Infrastructure.Factories.Services.Providers
{
    public class BossEnemyDependencyProviderFactory
    {
        public BossEnemyDependencyProvider Create(BossEnemy bossEnemy, IBossEnemyView bossEnemyView)
        {
            BossEnemyDependencyProvider provider = bossEnemyView.Provider;

            return provider;
        }
    }
}