using System;
using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Views;
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
using Sources.BoundedContexts.Upgrades.Infrastructure.Factories.Views;
using Sources.ECSBoundedContexts.StarUps.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.Collectors;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation
{
    public class GameplaySceneViewFactory : ISceneViewFactory
    {
        private readonly GameplayHud _gameplayHud;
        private readonly UiCollectorFactory _uiCollectorFactory;
        private readonly RootGameObject _rootGameObject;
        private readonly EnemySpawnerViewFactory _enemySpawnerViewFactory;
        private readonly IAudioService _audioService;
        private readonly BunkerViewFactory _bunkerViewFactory;
        private readonly NukeAbilityViewFactory _nukeAbilityViewFactory;
        private readonly AbilityApplierViewFactory _abilityApplierViewFactory;
        private readonly FlamethrowerAbilityViewFactory _flamethrowerAbilityViewFactory;
        private readonly EnemySpawnerUiFactory _enemySpawnerUiFactory;
        private readonly BunkerUiFactory _bunkerUiFactory;
        private readonly UpgradeViewFactory _upgradeViewFactory;
        private readonly GameplayModelsCreatorService _gameplayModelsCreatorService;
        private readonly GameplayModelsLoaderService _gameplayModelsLoaderService;
        private readonly PlayerWalletViewFactory _playerWalletViewFactory;
        private readonly IEcsGameStartUp _ecsGameStartUp;
        private readonly CharacterSpawnAbilityViewFactory _characterSpawnAbilityViewFactory;
        private readonly MusicChangerViewFactory _musicChangerViewFactory;
        private readonly SoundsChangerViewFactory _soundsChangerViewFactory;

        public GameplaySceneViewFactory(
            GameplayHud gameplayHud,
            UiCollectorFactory uiCollectorFactory,
            RootGameObject rootGameObject,
            EnemySpawnerViewFactory enemySpawnerViewFactory,
            CharacterSpawnAbilityViewFactory characterSpawnAbilityViewFactory,
            IAudioService audioService,
            BunkerViewFactory bunkerViewFactory,
            NukeAbilityViewFactory nukeAbilityViewFactory,
            AbilityApplierViewFactory abilityApplierViewFactory,
            FlamethrowerAbilityViewFactory flamethrowerAbilityViewFactory,
            EnemySpawnerUiFactory enemySpawnerUiFactory,
            BunkerUiFactory bunkerUiFactory,
            UpgradeViewFactory upgradeViewFactory,
            GameplayModelsCreatorService gameplayModelsCreatorService,
            GameplayModelsLoaderService gameplayModelsLoaderService,
            PlayerWalletViewFactory playerWalletViewFactory,
            IEcsGameStartUp ecsGameStartUp,
            MusicChangerViewFactory musicChangerViewFactory,
            SoundsChangerViewFactory soundsChangerViewFactory)
        {
            _gameplayHud = gameplayHud ?? throw new ArgumentNullException(nameof(gameplayHud));
            _uiCollectorFactory = uiCollectorFactory ?? throw new ArgumentNullException(nameof(uiCollectorFactory));
            _rootGameObject = rootGameObject ?? throw new ArgumentNullException(nameof(rootGameObject));
            _enemySpawnerViewFactory = enemySpawnerViewFactory ?? 
                                       throw new ArgumentNullException(nameof(enemySpawnerViewFactory));
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
            _gameplayModelsCreatorService = gameplayModelsCreatorService ?? 
                                            throw new ArgumentNullException(nameof(gameplayModelsCreatorService));
            _gameplayModelsLoaderService = gameplayModelsLoaderService ?? 
                                           throw new ArgumentNullException(nameof(gameplayModelsLoaderService));
            _playerWalletViewFactory = playerWalletViewFactory ??
                                       throw new ArgumentNullException(nameof(playerWalletViewFactory));
            _ecsGameStartUp = ecsGameStartUp ?? throw new ArgumentNullException(nameof(ecsGameStartUp));
            _characterSpawnAbilityViewFactory = characterSpawnAbilityViewFactory ?? 
                                                throw new ArgumentNullException(nameof(characterSpawnAbilityViewFactory));
            _musicChangerViewFactory = musicChangerViewFactory ??
                                       throw new ArgumentNullException(nameof(musicChangerViewFactory));
            _soundsChangerViewFactory = soundsChangerViewFactory ??
                                        throw new ArgumentNullException(nameof(soundsChangerViewFactory));
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
            _audioService.Construct(gameplayModel.Volume);
            _musicChangerViewFactory.Create(gameplayModel.Volume, _gameplayHud.MusicChangerView);
            _soundsChangerViewFactory.Create(gameplayModel.Volume, _gameplayHud.SoundsChangerView);
        }

        private GameplayModel Load(IScenePayload payload)
        {
            if (payload != null && payload.CanLoad)
                return _gameplayModelsLoaderService.Load();
            
            return _gameplayModelsCreatorService.Load();
        }
    }
}