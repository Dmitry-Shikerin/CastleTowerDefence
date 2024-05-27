using System;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.Controllers.Common;

namespace Sources.BoundedContexts.CharacterSpawners.Controllers
{
    public class CharacterSpawnerPresenter : PresenterBase
    {
        private readonly CharacterSpawner _characterSpawner;
        private readonly ICharacterSpawnerView _view;

        public CharacterSpawnerPresenter(CharacterSpawner characterSpawner, ICharacterSpawnerView view)
        {
            _characterSpawner = characterSpawner ?? throw new ArgumentNullException(nameof(characterSpawner));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public override void Enable()
        {
            
        }

        public override void Disable()
        {
            
        }
    }
}