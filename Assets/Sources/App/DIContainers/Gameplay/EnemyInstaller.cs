using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.Enemies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Services.Providers;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyBosses.Presentation.Implementation;
using Sources.Frameworks.Services.ObjectPools.Generic;
using Sources.Frameworks.Services.ObjectPools.Implementation;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class EnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEnemySpawnService>().To<EnemySpawnService>().AsSingle();
            Container.Bind<IObjectPool<EnemyView>>().To<ObjectPool<EnemyView>>().AsSingle();
            Container.Bind<EnemyDependencyProviderFactory>().AsSingle();
            Container.Bind<IEnemyViewFactory>().To<EnemyViewFactory>().AsSingle();

            Container.Bind<IBossEnemySpawnService>().To<BossEnemySpawnService>().AsSingle();
            Container.Bind<IObjectPool<BossEnemyView>>().To<ObjectPool<BossEnemyView>>().AsSingle();
            Container.Bind<BossEnemyDependencyProviderFactory>().AsSingle();
            Container.Bind<IBossEnemyViewFactory>().To<BossEnemyViewFactory>().AsSingle();
            
            Container.Bind<EnemyHealthPresenterFactory>().AsSingle();
            Container.Bind<EnemyHealthViewFactory>().AsSingle();
        }
    }
}