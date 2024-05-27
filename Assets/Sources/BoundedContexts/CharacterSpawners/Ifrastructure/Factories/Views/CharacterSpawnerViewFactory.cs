using System;
using Sources.BoundedContexts.CharacterMeleeSpawners.Domain;
using Sources.BoundedContexts.CharacterSpawners.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawners.Ifrastructure.Factories.Views
{
    public class CharacterSpawnerViewFactory
    {
        private readonly CharacterSpawnerPresenterFactory _presenterFactory;

        public CharacterSpawnerViewFactory(CharacterSpawnerPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public ICharacterSpawnerView Create(CharacterSpawner characterSpawner, CharacterSpawnerView view)
        {
            CharacterSpawnerPresenter presenter = _presenterFactory.Create(characterSpawner, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}