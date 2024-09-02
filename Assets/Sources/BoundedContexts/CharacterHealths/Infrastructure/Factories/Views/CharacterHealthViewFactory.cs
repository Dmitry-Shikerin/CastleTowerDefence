using Sources.BoundedContexts.CharacterHealths.Controllers;
using Sources.BoundedContexts.CharacterHealths.Domain;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;

namespace Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Views
{
    public class CharacterHealthViewFactory
    {
        public ICharacterHealthView Create(CharacterHealth characterHealth, CharacterHealthView view)
        {
            CharacterHealthPresenter presenter = new CharacterHealthPresenter(characterHealth, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}