using Sources.BoundedContexts.CharacterHealth.Presentation;
using Sources.BoundedContexts.CharacterHealths.Controllers;

namespace Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Controllers
{
    public class CharacterHealthPresenterFactory
    {
        public CharacterHealthPresenter Create(CharacterHealths.Domain.CharacterHealth characterHealth, CharacterHealthView view) =>
            new CharacterHealthPresenter(characterHealth, view);
    }
}