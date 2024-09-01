using System;
using System.Collections.Generic;
using System.Linq;
using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.FlamethrowerAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.PlayerWallets.Infrastructure.Factories.Views;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.Tutorials.Services.Interfaces;
using Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Views;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Prefabs.Interfaces;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Configs;
using Sources.Frameworks.MyGameCreator.Achivements.Domain.Models;
using Sources.Frameworks.UiFramework.Collectors;
using Sources.Frameworks.YandexSdcFramework.Advertisings.Services.Interfaces;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation
{
    public class GameplaySceneViewFactory : ISceneViewFactory
    {
        private readonly ILoadService _loadService;
        private readonly IAssetCollector _assetCollector;
        private readonly IEntityRepository _entityRepository;
        private readonly GameplayHud _gameplayHud;
        private readonly UiCollectorFactory _uiCollectorFactory;
        private readonly RootGameObject _rootGameObject;
        private readonly EnemySpawnerViewFactory _enemySpawnerViewFactory;
        private readonly BunkerViewFactory _bunkerViewFactory;
        private readonly NukeAbilityViewFactory _nukeAbilityViewFactory;
        private readonly AbilityApplierViewFactory _abilityApplierViewFactory;
        private readonly FlamethrowerAbilityViewFactory _flamethrowerAbilityViewFactory;
        private readonly EnemySpawnerUiFactory _enemySpawnerUiFactory;
        private readonly BunkerUiFactory _bunkerUiFactory;
        private readonly UpgradeViewFactory _upgradeViewFactory;
        private readonly GameplayModelsCreatorService _gameplayModelsCreatorService;
        private readonly GameplayModelsLoaderService _gameplayModelsLoaderService;
        private readonly IAdvertisingService _advertisingService;
        private readonly PlayerWalletViewFactory _playerWalletViewFactory;
        private readonly IEcsGameStartUp _ecsGameStartUp;
        private readonly CharacterSpawnAbilityViewFactory _characterSpawnAbilityViewFactory;
        private readonly VolumeViewFactory _volumeViewFactory;
        private readonly ITutorialService _tutorialService;

        public GameplaySceneViewFactory(
            ILoadService loadService,
            IAssetCollector assetCollector,
            IEntityRepository entityRepository,
            GameplayHud gameplayHud,
            UiCollectorFactory uiCollectorFactory,
            RootGameObject rootGameObject,
            EnemySpawnerViewFactory enemySpawnerViewFactory,
            CharacterSpawnAbilityViewFactory characterSpawnAbilityViewFactory,
            BunkerViewFactory bunkerViewFactory,
            NukeAbilityViewFactory nukeAbilityViewFactory,
            AbilityApplierViewFactory abilityApplierViewFactory,
            FlamethrowerAbilityViewFactory flamethrowerAbilityViewFactory,
            EnemySpawnerUiFactory enemySpawnerUiFactory,
            BunkerUiFactory bunkerUiFactory,
            UpgradeViewFactory upgradeViewFactory,
            GameplayModelsCreatorService gameplayModelsCreatorService,
            GameplayModelsLoaderService gameplayModelsLoaderService,
            IAdvertisingService advertisingService,
            PlayerWalletViewFactory playerWalletViewFactory,
            IEcsGameStartUp ecsGameStartUp,
            VolumeViewFactory volumeViewFactory,
            ITutorialService tutorialService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _assetCollector = assetCollector ?? throw new ArgumentNullException(nameof(assetCollector));
            _entityRepository = entityRepository ?? throw new ArgumentNullException(nameof(entityRepository));
            _gameplayHud = gameplayHud ?? throw new ArgumentNullException(nameof(gameplayHud));
            _uiCollectorFactory = uiCollectorFactory ?? throw new ArgumentNullException(nameof(uiCollectorFactory));
            _rootGameObject = rootGameObject ?? throw new ArgumentNullException(nameof(rootGameObject));
            _enemySpawnerViewFactory = enemySpawnerViewFactory ?? 
                                       throw new ArgumentNullException(nameof(enemySpawnerViewFactory));
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
            _gameplayModelsCreatorService = gameplayModelsCreatorService ?? 
                                            throw new ArgumentNullException(nameof(gameplayModelsCreatorService));
            _gameplayModelsLoaderService = gameplayModelsLoaderService ?? 
                                           throw new ArgumentNullException(nameof(gameplayModelsLoaderService));
            _advertisingService = advertisingService ?? throw new ArgumentNullException(nameof(advertisingService));
            _playerWalletViewFactory = playerWalletViewFactory ??
                                       throw new ArgumentNullException(nameof(playerWalletViewFactory));
            _ecsGameStartUp = ecsGameStartUp ?? throw new ArgumentNullException(nameof(ecsGameStartUp));
            _characterSpawnAbilityViewFactory = characterSpawnAbilityViewFactory ?? 
                                                throw new ArgumentNullException(nameof(characterSpawnAbilityViewFactory));
            _volumeViewFactory = volumeViewFactory ??
                                       throw new ArgumentNullException(nameof(volumeViewFactory));
            _tutorialService = tutorialService ?? throw new ArgumentNullException(nameof(tutorialService));
        }

        public void Create(IScenePayload payload)
        {
            GameplayModel gameplayModel = Load(payload);
            
            //PlayerWallet
            _playerWalletViewFactory.Create(_gameplayHud.PlayerWalletView);
            
            //Upgrades
            _upgradeViewFactory.Create(
                ModelId.HealthUpgrade, _gameplayHud.CharacterHealthUpgradeView);
            _upgradeViewFactory.Create(
                ModelId.AttackUpgrade, _gameplayHud.CharacterAttackUpgradeView);
            _upgradeViewFactory.Create(
                ModelId.NukeUpgrade, _gameplayHud.NukeAbilityUpgradeView);
            _upgradeViewFactory.Create(
                ModelId.FlamethrowerUpgrade, _gameplayHud.FlamethrowerAbilityUpgradeView);
            
            //Bunker
            IBunkerView bunkerView = _bunkerViewFactory.Create(_rootGameObject.BunkerView);
            _bunkerUiFactory.Create(_gameplayHud.BunkerUi);
            
            //Abilities
            _characterSpawnAbilityViewFactory.Create(_rootGameObject.CharacterSpawnAbilityView);
            _abilityApplierViewFactory.Create(ModelId.SpawnAbility, _gameplayHud.SpawnAbilityApplier);
            
            _nukeAbilityViewFactory.Create(_rootGameObject.NukeAbilityView);
            _abilityApplierViewFactory.Create(ModelId.NukeAbility, _gameplayHud.NukeAbilityApplier);

            _flamethrowerAbilityViewFactory.Create(_rootGameObject.FlamethrowerAbilityView);
            _abilityApplierViewFactory.Create(ModelId.FlamethrowerAbility, _gameplayHud.FlamethrowerAbilityApplier);

            //Enemies
            _rootGameObject.EnemySpawnerView.SetBunkerView(bunkerView);
            _enemySpawnerViewFactory.Create(_rootGameObject.EnemySpawnerView);
            _enemySpawnerUiFactory.Create(_gameplayHud.EnemySpawnerUi);

            //UiCollector
            _uiCollectorFactory.Create();

            //Volume
            _volumeViewFactory.Create(gameplayModel.MusicVolume, _gameplayHud.MusicVolumeView);
            _volumeViewFactory.Create(gameplayModel.SoundsVolume, _gameplayHud.SoundVolumeView);
            
            //HealthBooster
            _gameplayHud.HealthBoosterView.Construct(_entityRepository);
            
            //Achievements
            List<Achievement> achievements = _entityRepository.GetAll<Achievement>(ModelId.AchievementModels).ToList();

            if (achievements.Count != _gameplayHud.AchievementViews.Count)
                throw new IndexOutOfRangeException(nameof(achievements));

            for (int i = 0; i < achievements.Count; i++)
            {
                AchievementConfig config = _assetCollector
                    .Get<AchievementConfigCollector>()
                    .Configs
                    .First(config => config.Id == achievements[i].Id);
                _gameplayHud.AchievementViews[i].Construct(achievements[i], config);
            }
            
            //Tutorial
            _tutorialService.Construct(gameplayModel.Tutorial);
            
            //advertising
            _advertisingService.Construct(gameplayModel.HealthBooster);
        }

        private GameplayModel Load(IScenePayload payload)
        {
            // if (payload != null && payload.CanLoad)
            //     return _gameplayModelsLoaderService.Load();
            if (_loadService.HasKey(ModelId.PlayerWallet))
                return _gameplayModelsLoaderService.Load();
            
            return _gameplayModelsCreatorService.Load();
        }
    }
}