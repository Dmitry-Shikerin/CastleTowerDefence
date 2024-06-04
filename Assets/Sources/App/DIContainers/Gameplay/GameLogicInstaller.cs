using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRotations.Services.Implementation;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Views;
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
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.Frameworks.GameServices.ObjectPools.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameLogicInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Bunkers
            Container.Bind<BunkerViewFactory>().AsSingle();
            Container.Bind<BunkerPresenterFactory>().AsSingle();
            
            //Healths
            Container.Bind<HealthBarPresenterFactory>().AsSingle();
            Container.Bind<HealthBarViewFactory>().AsSingle();
            
            //CharactersSpawners
            Container.Bind<CharacterSpawnerPresenterFactory>().AsSingle();
            Container.Bind<CharacterSpawnerViewFactory>().AsSingle();

            //Characters
            Container.Bind<ICharacterRotationService>().To<CharacterRotationService>().AsSingle();
            
            Container.Bind<IObjectPool<CharacterMeleeView>>().To<ObjectPool<CharacterMeleeView>>().AsSingle();
            Container.Bind<ICharacterMeleeSpawnService>().To<CharacterMeleeSpawnService>().AsSingle();
            Container.Bind<CharacterMeleeDependencyProviderFactory>().AsSingle();
            Container.Bind<ICharacterMeleeViewFactory>().To<CharacterMeleeViewFactory>().AsSingle();

            Container.Bind<IObjectPool<CharacterRangeView>>().To<ObjectPool<CharacterRangeView>>().AsSingle();
            Container.Bind<ICharacterRangeSpawnService>().To<CharacterRangeSpawnService>().AsSingle();
            Container.Bind<CharacterRangeDependencyProviderFactory>().AsSingle();
            Container.Bind<ICharacterRangeViewFactory>().To<CharacterRangeViewFactory>().AsSingle();

            Container.Bind<CharacterHealthPresenterFactory>().AsSingle();
            Container.Bind<CharacterHealthViewFactory>().AsSingle();
            
            //EnemiesSpawners
            Container.Bind<EnemySpawnerPresenterFactory>().AsSingle();
            Container.Bind<EnemySpawnerViewFactory>().AsSingle();
            
            //Enemies
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