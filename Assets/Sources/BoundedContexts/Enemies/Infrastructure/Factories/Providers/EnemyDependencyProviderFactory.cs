using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers
{
    public class EnemyDependencyProviderFactory
    {
        public EnemyDependencyProvider Create(Enemy enemy, EnemyView view)
        {
            EnemyDependencyProvider provider = view.Provider;
            provider.Construct(
                enemy, 
                view,
                view.Animation);
            
            return provider;
        }
    }
}