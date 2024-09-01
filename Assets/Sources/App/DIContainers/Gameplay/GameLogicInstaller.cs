using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.BurnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Characters.Services.Implementation;
using Sources.BoundedContexts.Characters.Services.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.EnemyBosses.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.ExplosionBodies.Infrastructure.Factories.Views.Implementation;
using Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.GameCompleted.Infrastructure.Services.Implementation;
using Sources.BoundedContexts.GameCompleted.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.GameOvers.Infrastructure.Services.Implementation;
using Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.PlayerWallets.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Views;
using Zenject;

namespace Sources.App.DIContainers.Gameplay
{
    public class GameLogicInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //GameOvers
            Container.Bind<IGameOverService>().To<GameOverService>().AsSingle();
            
            //GameCompleted
            Container.Bind<IGameCompletedService>().To<GameCompletedService>().AsSingle();
            
            //PlayerWallet
            Container.Bind<PlayerWalletViewFactory>().AsSingle();
            
            //Bunkers
            Container.Bind<BunkerViewFactory>().AsSingle();
            Container.Bind<BunkerPresenterFactory>().AsSingle();

            Container.Bind<BunkerUiPresenterFactory>().AsSingle();
            Container.Bind<BunkerUiFactory>().AsSingle();
            
            //Healths
            Container.Bind<HealthBarViewFactory>().AsSingle();
            
            //Characters
            Container.Bind<ICharacterRotationService>().To<CharacterRotationService>().AsSingle();
            
            Container.Bind<CharacterMeleeViewFactory>().AsSingle();

            Container.Bind<CharacterRangeViewFactory>().AsSingle();

            Container.Bind<CharacterHealthViewFactory>().AsSingle();
            
            //EnemiesSpawners
            Container.Bind<EnemySpawnerPresenterFactory>().AsSingle();
            Container.Bind<EnemySpawnerViewFactory>().AsSingle();

            Container.Bind<EnemySpawnerUiFactory>().AsSingle();
            
            //Enemies
            Container.Bind<EnemyViewFactory>().AsSingle();

            Container.Bind<EnemyBossViewFactory>().AsSingle();

            Container.Bind<EnemyKamikazeViewFactory>().AsSingle();
            
            Container.Bind<EnemyHealthPresenterFactory>().AsSingle();
            Container.Bind<EnemyHealthViewFactory>().AsSingle();
            
            //ExplosionBodyBloody
            Container.Bind<ExplosionBodyBloodyViewFactory>().AsSingle();
            Container.Bind<ExplosionBodyViewFactory>().AsSingle();
            
            //ApplyAbilities
            Container.Bind<AbilityApplierPresenterFactory>().AsSingle();
            Container.Bind<AbilityApplierViewFactory>().AsSingle();
            
            Container.Bind<NukeAbilityPresenterFactory>().AsSingle();
            Container.Bind<NukeAbilityViewFactory>().AsSingle();
            
            Container.Bind<CharacterSpawnAbilityPresenterFactory>().AsSingle();
            Container.Bind<CharacterSpawnAbilityViewFactory>().AsSingle();

            Container.Bind<FlamethrowerAbilityViewFactory>().AsSingle();
            
            //Abilities
            Container.Bind<BurnAbilityPresenterFactory>().AsSingle();
            Container.Bind<BurnAbilityViewFactory>().AsSingle();
            
            //Upgrades
            Container.Bind<UpgradeViewFactory>().AsSingle();
        }
    }
}