using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Views;
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
using Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Views;
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
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Implementation;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.ExplosionBodies.Presentation.Implementation;
using Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Views;
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

            Container.Bind<EnemySpawnerUiPresenterFactory>().AsSingle();
            Container.Bind<EnemySpawnerUiFactory>().AsSingle();
            
            //Enemies
            Container.Bind<IEnemySpawnService>().To<EnemySpawnService>().AsSingle();
            Container.Bind<IObjectPool<EnemyView>>().To<ObjectPool<EnemyView>>().AsSingle();
            Container.Bind<EnemyDependencyProviderFactory>().AsSingle();
            Container.Bind<IEnemyViewFactory>().To<EnemyViewFactory>().AsSingle();

            Container.Bind<IBossEnemySpawnService>().To<BossEnemySpawnService>().AsSingle();
            Container.Bind<IObjectPool<BossEnemyView>>().To<ObjectPool<BossEnemyView>>().AsSingle();
            Container.Bind<BossEnemyDependencyProviderFactory>().AsSingle();
            Container.Bind<IBossEnemyViewFactory>().To<BossEnemyViewFactory>().AsSingle();

            Container.Bind<IEnemyKamikazeSpawnService>().To<EnemyKamikazeSpawnService>().AsSingle();
            Container.Bind<IObjectPool<EnemyKamikazeView>>().To<ObjectPool<EnemyKamikazeView>>().AsSingle();
            Container.Bind<EnemyKamikazeDependencyProviderFactory>().AsSingle();
            Container.Bind<IEnemyKamikazeViewFactory>().To<EnemyKamikazeViewFactory>().AsSingle();
            
            Container.Bind<EnemyHealthPresenterFactory>().AsSingle();
            Container.Bind<EnemyHealthViewFactory>().AsSingle();
            
            //ExplosionBodyBloody
            Container.Bind<IObjectPool<ExplosionBodyBloodyView>>().To<ObjectPool<ExplosionBodyBloodyView>>().AsSingle();
            Container.Bind<IExplosionBodyBloodyViewFactory>().To<ExplosionBodyBloodyViewFactory>().AsSingle();
            Container.Bind<IExplosionBodyBloodySpawnService>().To<ExplosionBodyBloodySpawnService>().AsSingle();

            Container.Bind<IObjectPool<ExplosionBodyView>>().To<ObjectPool<ExplosionBodyView>>().AsSingle();
            Container.Bind<IExplosionBodyViewFactory>().To<ExplosionBodyViewFactory>().AsSingle();
            Container.Bind<IExplosionBodySpawnService>().To<ExplosionBodySpawnService>().AsSingle();
            
            //Abilities
            Container.Bind<AbilityApplierPresenterFactory>().AsSingle();
            Container.Bind<AbilityApplierViewFactory>().AsSingle();
            
            Container.Bind<NukeAbilityPresenterFactory>().AsSingle();
            Container.Bind<NukeAbilityViewFactory>().AsSingle();
            
            Container.Bind<CharacterSpawnAbilityPresenterFactory>().AsSingle();
            Container.Bind<CharacterSpawnAbilityViewFactory>().AsSingle();

            Container.Bind<FlamethrowerAbilityPresenterFactory>().AsSingle();
            Container.Bind<FlamethrowerAbilityViewFactory>().AsSingle();
            
            //Upgrades
            Container.Bind<UpgradePresenterFactory>().AsSingle();
            Container.Bind<UpgradeViewFactory>().AsSingle();
        }
    }
}