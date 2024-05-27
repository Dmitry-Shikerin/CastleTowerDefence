using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterSpawners.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Controllers
{
    public class CharacterSpawnerPresenterFactory
    {
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;

        public CharacterSpawnerPresenterFactory(
            ICharacterMeleeSpawnService characterMeleeSpawnService)
        {
            _characterMeleeSpawnService = characterMeleeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
        }

        public CharacterSpawnerPresenter Create(CharacterSpawner characterSpawner, ICharacterSpawnerView view)
        {
            return new CharacterSpawnerPresenter(
                characterSpawner,
                view,
                _characterMeleeSpawnService);
        }
    }
}