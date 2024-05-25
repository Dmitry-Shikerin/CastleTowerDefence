using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.Characters.Controllers;

namespace Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Controllers
{
    public class CharacterHealthPresenterFactory
    {
        public CharacterHealthPresenter Create(Characters.CharacterHealth characterHealth, CharacterHealthView view) =>
            new CharacterHealthPresenter(characterHealth, view);
    }
}