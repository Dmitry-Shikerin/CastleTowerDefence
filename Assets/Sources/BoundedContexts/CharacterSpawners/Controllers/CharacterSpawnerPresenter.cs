using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.CharacterSpawners.Controllers
{
    public class CharacterSpawnerPresenter : PresenterBase
    {
        private readonly CharacterSpawner _characterSpawner;
        private readonly ICharacterSpawnerView _view;
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;
        private readonly ICharacterRangeSpawnService _characterRangeSpawnService;

        public CharacterSpawnerPresenter(
            CharacterSpawner characterSpawner, 
            ICharacterSpawnerView view,
            ICharacterMeleeSpawnService characterMeleeSpawnService,
            ICharacterRangeSpawnService characterRangeSpawnService)
        {
            _characterSpawner = characterSpawner ?? throw new ArgumentNullException(nameof(characterSpawner));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _characterMeleeSpawnService = characterMeleeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
            _characterRangeSpawnService = characterRangeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterRangeSpawnService));
        }

        public override void Enable()
        {
            SpawnMelee();
            SpawnRange();
        }

        public override void Disable()
        {
            
        }

        private void SpawnMelee()
        {
            foreach (ICharacterSpawnPoint spawnPoint in _view.MeleeSpawnPoints)
            {
                ICharacterMeleeView view = _characterMeleeSpawnService.Spawn(spawnPoint.Position);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }
        
        private void SpawnRange()
        {
            foreach (ICharacterSpawnPoint spawnPoint in _view.RangeSpawnPoints)
            {
                ICharacterRangeView view =_characterRangeSpawnService.Spawn(spawnPoint.Position);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }
    }
}