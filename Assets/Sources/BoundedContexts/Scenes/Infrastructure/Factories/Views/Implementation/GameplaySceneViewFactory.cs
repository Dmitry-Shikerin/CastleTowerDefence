using System;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Views;
using Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
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
        private readonly UiCollectorFactory _uiCollectorFactory;
        private readonly RootGameObject _rootGameObject;
        private readonly EnemySpawnerViewFactory _enemySpawnerViewFactory;
        private readonly ICharacterMeleeViewFactory _characterMeleeViewFactory;
        private readonly IAudioService _audioService;
        private readonly BunkerViewFactory _bunkerViewFactory;
        private readonly CharacterSpawnerViewFactory _characterSpawnerViewFactory;
        private readonly IEnemyViewFactory _enemyViewFactory;

        public GameplaySceneViewFactory(
            UiCollectorFactory uiCollectorFactory,
            RootGameObject rootGameObject,
            EnemySpawnerViewFactory enemySpawnerViewFactory,
            CharacterSpawnerViewFactory characterSpawnerViewFactory,
            IEnemyViewFactory enemyViewFactory,
            ICharacterMeleeViewFactory characterMeleeViewFactory,
            IAudioService audioService,
            BunkerViewFactory bunkerViewFactory)
        {
            _uiCollectorFactory = uiCollectorFactory ?? throw new ArgumentNullException(nameof(uiCollectorFactory));
            _rootGameObject = rootGameObject ?? throw new ArgumentNullException(nameof(rootGameObject));
            _enemySpawnerViewFactory = enemySpawnerViewFactory ?? 
                                       throw new ArgumentNullException(nameof(enemySpawnerViewFactory));
            _characterMeleeViewFactory = characterMeleeViewFactory ?? 
                                         throw new ArgumentNullException(nameof(characterMeleeViewFactory));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _bunkerViewFactory = bunkerViewFactory ?? throw new ArgumentNullException(nameof(bunkerViewFactory));
            _characterSpawnerViewFactory = characterSpawnerViewFactory ?? 
                                           throw new ArgumentNullException(nameof(characterSpawnerViewFactory));
            _enemyViewFactory = enemyViewFactory ?? throw new ArgumentNullException(nameof(enemyViewFactory));
        }

        public void Create(IScenePayload payload)
        {
            Bunker bunker = new Bunker(15);
            IBunkerView bunkerView = _bunkerViewFactory.Create(bunker, _rootGameObject.BunkerView);
            
            //Characters
            CharacterSpawner characterSpawner = new CharacterSpawner();
            _characterSpawnerViewFactory.Create(characterSpawner, _rootGameObject.CharacterSpawnerView);

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
            //Bunker
            _audioService.Construct(volume);
        }
    }
}