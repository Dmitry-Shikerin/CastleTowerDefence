using Sources.BoundedContexts.CharacterHealths.Controllers;
using Sources.BoundedContexts.CharacterHealths.Presentation;

namespace Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Controllers
{
    public class CharacterHealthPresenterFactory
    {
        public CharacterHealthPresenter Create(
            CharacterHealths.Domain.CharacterHealth characterHealth, 
            CharacterHealthView view)
        {
            return new CharacterHealthPresenter(characterHealth, view);
        }
    }
}