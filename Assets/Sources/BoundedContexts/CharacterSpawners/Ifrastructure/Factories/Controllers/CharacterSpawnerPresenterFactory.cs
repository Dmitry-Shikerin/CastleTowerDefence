using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterSpawners.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Controllers
{
    public class CharacterSpawnerPresenterFactory
    {
        public CharacterSpawnerPresenter Create(CharacterSpawner characterSpawner, ICharacterSpawnerView view)
        {
            return new CharacterSpawnerPresenter(characterSpawner, view);
        }
    }
}