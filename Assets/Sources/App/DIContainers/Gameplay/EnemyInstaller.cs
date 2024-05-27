using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.Frameworks.Services.ObjectPools.Generic;
using Sources.Frameworks.Services.ObjectPools.Implementation;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class EnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IObjectPool<EnemyView>>().To<ObjectPool<EnemyView>>().AsSingle();
            
            Container.Bind<EnemyHealthPresenterFactory>().AsSingle();
            Container.Bind<EnemyHealthViewFactory>().AsSingle();

            Container.Bind<EnemyDependencyProviderFactory>().AsSingle();
            Container.Bind<EnemyViewFactory>().AsSingle();
        }
    }
}