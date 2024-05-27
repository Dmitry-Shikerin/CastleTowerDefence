using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.SpawnPoints.PresentationInterfaces;
using Sources.Controllers.Common;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterSpawners.Controllers
{
    public class CharacterSpawnerPresenter : PresenterBase
    {
        private readonly CharacterSpawner _characterSpawner;
        private readonly ICharacterSpawnerView _view;
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;

        public CharacterSpawnerPresenter(
            CharacterSpawner characterSpawner, 
            ICharacterSpawnerView view,
            ICharacterMeleeSpawnService characterMeleeSpawnService)
        {
            _characterSpawner = characterSpawner ?? throw new ArgumentNullException(nameof(characterSpawner));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _characterMeleeSpawnService = characterMeleeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
        }

        public override void Enable()
        {
            SpawnMelee();
        }

        public override void Disable()
        {
            
        }

        private void SpawnMelee()
        {
            foreach (ISpawnPoint spawnPoint in _view.MeleeSpawnPoints)
            {
                _characterMeleeSpawnService.Spawn(spawnPoint.Position);
            }
        }
    }
}