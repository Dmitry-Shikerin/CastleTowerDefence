using System;
using Sources.BoundedContexts.CharacterSpawnAbilities.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Views
{
    public class CharacterSpawnAbilityViewFactory
    {
        private readonly CharacterSpawnAbilityPresenterFactory _presenterFactory;

        public CharacterSpawnAbilityViewFactory(CharacterSpawnAbilityPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? 
                                throw new ArgumentNullException(nameof(presenterFactory));
        }

        public ICharacterSpawnAbilityView Create(CharacterSpawnAbility characterSpawnAbility, CharacterSpawnAbilityView view)
        {
            CharacterSpawnAbilityPresenter presenter = _presenterFactory.Create(characterSpawnAbility, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}