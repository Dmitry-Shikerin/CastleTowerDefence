using System;
using Sources.BoundedContexts.CharacterSpawnAbilities.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Infrastructure.Factories.Views
{
    public class CharacterSpawnAbilityViewFactory
    {
        private readonly CharacterSpawnAbilityPresenterFactory _presenterFactory;

        public CharacterSpawnAbilityViewFactory(CharacterSpawnAbilityPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? 
                                throw new ArgumentNullException(nameof(presenterFactory));
        }

        public ICharacterSpawnAbilityView Create(CharacterSpawnAbilityView view)
        {
            CharacterSpawnAbilityPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);
            
            return view;
        }
    }
}