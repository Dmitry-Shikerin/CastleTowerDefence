using System;
using System.Linq;
using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Configs;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models;
using Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Prefabs;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Upgrades.Domain.Configs;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Views;
using Sources.Domain.Models.Data;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation
{
    public class GameplaySceneViewFactory : ISceneViewFactory
    {
        private readonly GameplayHud _gameplayHud;
        private readonly UiCollectorFactory _uiCollectorFactory;
        private readonly RootGameObject _rootGameObject;
        private readonly EnemySpawnerViewFactory _enemySpawnerViewFactory;
        private readonly ICharacterMeleeViewFactory _characterMeleeViewFactory;
        private readonly IAudioService _audioService;
        private readonly BunkerViewFactory _bunkerViewFactory;
        private readonly NukeAbilityViewFactory _nukeAbilityViewFactory;
        private readonly AbilityApplierViewFactory _abilityApplierViewFactory;
        private readonly FlamethrowerAbilityViewFactory _flamethrowerAbilityViewFactory;
        private readonly EnemySpawnerUiFactory _enemySpawnerUiFactory;
        private readonly BunkerUiFactory _bunkerUiFactory;
        private readonly UpgradeViewFactory _upgradeViewFactory;
        private readonly UpgradeConfigContainer _upgradeConfigContainer;
        private readonly CharacterSpawnAbilityViewFactory _characterSpawnAbilityViewFactory;
        private readonly IEnemyViewFactory _enemyViewFactory;

        public GameplaySceneViewFactory(
            GameplayHud gameplayHud,
            UiCollectorFactory uiCollectorFactory,
            RootGameObject rootGameObject,
            EnemySpawnerViewFactory enemySpawnerViewFactory,
            CharacterSpawnAbilityViewFactory characterSpawnAbilityViewFactory,
            IEnemyViewFactory enemyViewFactory,
            ICharacterMeleeViewFactory characterMeleeViewFactory,
            IAudioService audioService,
            BunkerViewFactory bunkerViewFactory,
            NukeAbilityViewFactory nukeAbilityViewFactory,
            AbilityApplierViewFactory abilityApplierViewFactory,
            FlamethrowerAbilityViewFactory flamethrowerAbilityViewFactory,
            EnemySpawnerUiFactory enemySpawnerUiFactory,
            BunkerUiFactory bunkerUiFactory,
            UpgradeViewFactory upgradeViewFactory,
            UpgradeConfigContainer upgradeConfigContainer)
        {
            _gameplayHud = gameplayHud ?? throw new ArgumentNullException(nameof(gameplayHud));
            _uiCollectorFactory = uiCollectorFactory ?? throw new ArgumentNullException(nameof(uiCollectorFactory));
            _rootGameObject = rootGameObject ?? throw new ArgumentNullException(nameof(rootGameObject));
            _enemySpawnerViewFactory = enemySpawnerViewFactory ?? 
                                       throw new ArgumentNullException(nameof(enemySpawnerViewFactory));
            _characterMeleeViewFactory = characterMeleeViewFactory ?? 
                                         throw new ArgumentNullException(nameof(characterMeleeViewFactory));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _bunkerViewFactory = bunkerViewFactory ?? throw new ArgumentNullException(nameof(bunkerViewFactory));
            _nukeAbilityViewFactory = nukeAbilityViewFactory ?? 
                                      throw new ArgumentNullException(nameof(nukeAbilityViewFactory));
            _abilityApplierViewFactory = abilityApplierViewFactory ?? 
                                         throw new ArgumentNullException(nameof(abilityApplierViewFactory));
            _flamethrowerAbilityViewFactory = flamethrowerAbilityViewFactory ?? 
                                              throw new ArgumentNullException(nameof(flamethrowerAbilityViewFactory));
            _enemySpawnerUiFactory = enemySpawnerUiFactory ?? 
                                     throw new ArgumentNullException(nameof(enemySpawnerUiFactory));
            _bunkerUiFactory = bunkerUiFactory ?? throw new ArgumentNullException(nameof(bunkerUiFactory));
            _upgradeViewFactory = upgradeViewFactory ?? throw new ArgumentNullException(nameof(upgradeViewFactory));
            _upgradeConfigContainer = upgradeConfigContainer ?? throw new ArgumentNullException(nameof(upgradeConfigContainer));
            _characterSpawnAbilityViewFactory = characterSpawnAbilityViewFactory ?? 
                                                throw new ArgumentNullException(nameof(characterSpawnAbilityViewFactory));
            _enemyViewFactory = enemyViewFactory ?? 
                                throw new ArgumentNullException(nameof(enemyViewFactory));
        }

        public void Create(IScenePayload payload)
        {
            //PlayerWallet
            PlayerWallet playerWallet = new PlayerWallet(50, "PlayerWallet");
            
            //Upgrades
            Upgrade characterHealthUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == "Health"));
            _upgradeViewFactory.Create(
                characterHealthUpgrade, 
                playerWallet,
                _gameplayHud.CharacterHealthUpgradeView);
            Upgrade characterAttackUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == "Attack"));
            _upgradeViewFactory.Create(
                characterAttackUpgrade, 
                playerWallet,
                _gameplayHud.CharacterAttackUpgradeView);
            Upgrade nukeAbilityUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == "NukeAbility"));
            _upgradeViewFactory.Create(
                nukeAbilityUpgrade, 
                playerWallet,
                _gameplayHud.NukeAbilityUpgradeView);
            Upgrade flamethrowerAbilityUpgrade = new Upgrade(
                _upgradeConfigContainer.UpgradeConfigs.First(config => config.Id == "Flamethrower"));
            _upgradeViewFactory.Create(
                flamethrowerAbilityUpgrade, 
                playerWallet,
                _gameplayHud.FlamethrowerAbilityUpgradeView);
            
            //Bunker
            Bunker bunker = new Bunker(15);
            IBunkerView bunkerView = _bunkerViewFactory.Create(bunker, _rootGameObject.BunkerView);
            _bunkerUiFactory.Create(bunker, _gameplayHud.BunkerUi);
            
            //Abilities
            CharacterSpawnAbility characterSpawnAbility = new CharacterSpawnAbility();
            _characterSpawnAbilityViewFactory.Create(
                characterSpawnAbility, 
                characterHealthUpgrade,
                _rootGameObject.CharacterSpawnAbilityView);
            _abilityApplierViewFactory.Create(characterSpawnAbility, _gameplayHud.SpawnAbilityApplier);
            
            NukeAbility nukeAbility = new NukeAbility(nukeAbilityUpgrade);
            _nukeAbilityViewFactory.Create(nukeAbility, _rootGameObject.NukeAbilityView);
            _abilityApplierViewFactory.Create(nukeAbility, _gameplayHud.NukeAbilityApplier);

            FlamethrowerAbility flamethrowerAbility = new FlamethrowerAbility(flamethrowerAbilityUpgrade);
            _flamethrowerAbilityViewFactory.Create(flamethrowerAbility, _rootGameObject.FlamethrowerAbilityView);
            _abilityApplierViewFactory.Create(flamethrowerAbility, _gameplayHud.FlamethrowerAbilityApplier);

            //Enemies
            EnemySpawnerConfig enemySpawnerConfig = 
                Resources.Load<EnemySpawnerConfig>(
                PrefabPath.EnemySpawnerConfigContainer);
            EnemySpawner enemySpawner = new EnemySpawner(enemySpawnerConfig);
            _rootGameObject.EnemySpawnerView.SetBunkerView(bunkerView);
            _enemySpawnerViewFactory.Create(
                enemySpawner, 
                new KillEnemyCounter(new KillEnemyCounterDto()), 
                playerWallet,
                _rootGameObject.EnemySpawnerView);
            _enemySpawnerUiFactory.Create(enemySpawner, _gameplayHud.EnemySpawnerUi);

            //UiCollector
            _uiCollectorFactory.Create();

            //Volume
            Volume volume = new Volume();
            _audioService.Construct(volume);
        }
    }
}