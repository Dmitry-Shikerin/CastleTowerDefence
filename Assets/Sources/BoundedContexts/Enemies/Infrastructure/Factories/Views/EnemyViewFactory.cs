using System;
using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Presentation;
using Zenject;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views
{
    public class EnemyViewFactory
    {
        private readonly DiContainer _container;

        public EnemyViewFactory(
            DiContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        
        public IEnemyView Create(Enemy enemy, EnemyView view)
        {
            view.Provider.Construct(enemy, view);
            _container.Inject(view.Provider);
            view.FsmOwner.StartBehaviour();
            
            return view;
        }
    }
}