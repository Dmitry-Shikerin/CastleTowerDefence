using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterSpawnAbilities.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Infrastructure.Factories.Controllers
{
    public class CharacterSpawnAbilityPresenterFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly IEntityRepository _entityRepository;
        private readonly CharacterMeleeViewFactory _characterMeleeViewFactory;
        private readonly CharacterRangeViewFactory _characterRangeViewFactory;

        public CharacterSpawnAbilityPresenterFactory(
            IPoolManager poolManager,
            IEntityRepository entityRepository,
            CharacterMeleeViewFactory characterMeleeViewFactory,
            CharacterRangeViewFactory characterRangeViewFactory)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _characterMeleeViewFactory = characterMeleeViewFactory ?? throw new ArgumentNullException(nameof(characterMeleeViewFactory));
            _characterRangeViewFactory = characterRangeViewFactory ?? throw new ArgumentNullException(nameof(characterRangeViewFactory));
        }

        public CharacterSpawnAbilityPresenter Create(ICharacterSpawnAbilityView view)
        {
            return new CharacterSpawnAbilityPresenter(
                _poolManager,
                _entityRepository,
                view,
                _characterMeleeViewFactory,
                _characterRangeViewFactory);
        }
    }
}