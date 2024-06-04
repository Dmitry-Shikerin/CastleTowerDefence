using System;
using Sources.BoundedContexts.Abilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.NukeAbilities.Domain.Models;
using Sources.BoundedContexts.NukeAbilities.Infrastructure.Factories.Views;
using Sources.BoundedContexts.RootGameObjects.Presentation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.Domain.Models.Data;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
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
        private readonly ICharacterMeleeViewFactory _characterMeleeViewFactory;
        private readonly IAudioService _audioService;
        private readonly BunkerViewFactory _bunkerViewFactory;
        private readonly NukeAbilityViewFactory _nukeAbilityViewFactory;
        private readonly AbilityApplierViewFactory _abilityApplierViewFactory;
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
            AbilityApplierViewFactory abilityApplierViewFactory)
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
            _characterSpawnAbilityViewFactory = characterSpawnAbilityViewFactory ?? 
                                                throw new ArgumentNullException(nameof(characterSpawnAbilityViewFactory));
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
        }

        public void Create(IScenePayload payload)
        {
            //Bunker
            Bunker bunker = new Bunker(15);
            IBunkerView bunkerView = _bunkerViewFactory.Create(bunker, _rootGameObject.BunkerView);
            
            //Abilities
            CharacterSpawnAbility characterSpawnAbility = new CharacterSpawnAbility();
            _characterSpawnAbilityViewFactory.Create(characterSpawnAbility, _rootGameObject.CharacterSpawnAbilityView);
            _abilityApplierViewFactory.Create(characterSpawnAbility, _gameplayHud.SpawnAbilityApplier);
            
            NukeAbility nukeAbility = new NukeAbility();
            _nukeAbilityViewFactory.Create(nukeAbility, _rootGameObject.NukeAbilityView);
            _abilityApplierViewFactory.Create(nukeAbility, _gameplayHud.NukeAbilityApplier);

            //Enemies
            EnemySpawner enemySpawner = new EnemySpawner();
            _rootGameObject.EnemySpawnerView.SetBunkerView(bunkerView);
            _enemySpawnerViewFactory.Create(
                enemySpawner, 
                new KillEnemyCounter(new KillEnemyCounterDto()), 
                _rootGameObject.EnemySpawnerView);

            //UiCollector
            _uiCollectorFactory.Create();

            //Volume
            Volume volume = new Volume();
            _audioService.Construct(volume);
        }
    }
}