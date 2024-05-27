using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Controllers
{
    public class CharacterSpawnerPresenterFactory
    {
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;
        private readonly ICharacterRangeSpawnService _characterRangeSpawnService;

        public CharacterSpawnerPresenterFactory(
            ICharacterMeleeSpawnService characterMeleeSpawnService,
            ICharacterRangeSpawnService characterRangeSpawnService)
        {
            _characterMeleeSpawnService = characterMeleeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
            _characterRangeSpawnService = characterRangeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterRangeSpawnService));
        }

        public CharacterSpawnerPresenter Create(CharacterSpawner characterSpawner, ICharacterSpawnerView view)
        {
            return new CharacterSpawnerPresenter(
                characterSpawner,
                view,
                _characterMeleeSpawnService,
                _characterRangeSpawnService);
        }
    }
}