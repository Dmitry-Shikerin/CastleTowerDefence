using System;
using Sources.BoundedContexts.CharacterHealths.Controllers;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterHealths.Presentation;
using Sources.BoundedContexts.CharacterHealths.PresentationInterfaces;

namespace Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Views
{
    public class CharacterHealthViewFactory
    {
        private readonly CharacterHealthPresenterFactory _presenterFactory;

        public CharacterHealthViewFactory(CharacterHealthPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public ICharacterHealthView Create(CharacterHealths.Domain.CharacterHealth characterHealth, CharacterHealthView view)
        {
            CharacterHealthPresenter presenter = _presenterFactory.Create(characterHealth, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}