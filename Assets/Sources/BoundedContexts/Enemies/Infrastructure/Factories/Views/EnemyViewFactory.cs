using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Providers;
using Sources.BoundedContexts.Enemies.Presentation;
using Zenject;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views
{
    public class EnemyViewFactory
    {
        private readonly EnemyDependencyProviderFactory _providerFactory;

        public EnemyViewFactory(
            EnemyDependencyProviderFactory providerFactory)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
        }
        
        public IEnemyView Create(Enemy enemy, EnemyView view)
        {
            _providerFactory.Create(enemy, view);
            view.FsmOwner.StartBehaviour();
            
            return view;
        }
    }
}