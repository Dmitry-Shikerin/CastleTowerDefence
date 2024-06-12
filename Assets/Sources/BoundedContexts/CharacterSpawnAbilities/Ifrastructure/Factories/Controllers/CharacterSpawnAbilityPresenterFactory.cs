using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawnAbilities.Controllers;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Controllers
{
    public class CharacterSpawnAbilityPresenterFactory
    {
        private readonly IEntityRepository _entityRepository;
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;
        private readonly ICharacterRangeSpawnService _characterRangeSpawnService;
        private readonly IObjectPool<CharacterMeleeView> _characterMeleePool;
        private readonly IObjectPool<CharacterRangeView> _characterRangePool;

        public CharacterSpawnAbilityPresenterFactory(
            IEntityRepository entityRepository,
            ICharacterMeleeSpawnService characterMeleeSpawnService,
            ICharacterRangeSpawnService characterRangeSpawnService,
            IObjectPool<CharacterMeleeView> characterMeleePool,
            IObjectPool<CharacterRangeView> characterRangePool)
        {
            _entityRepository = entityRepository ?? 
                                throw new ArgumentNullException(nameof(entityRepository));
            _characterMeleeSpawnService = characterMeleeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
            _characterRangeSpawnService = characterRangeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterRangeSpawnService));
            _characterMeleePool = characterMeleePool ?? throw new ArgumentNullException(nameof(characterMeleePool));
            _characterRangePool = characterRangePool ?? throw new ArgumentNullException(nameof(characterRangePool));
        }

        public CharacterSpawnAbilityPresenter Create(ICharacterSpawnAbilityView view)
        {
            return new CharacterSpawnAbilityPresenter(
                _entityRepository,
                view,
                _characterMeleeSpawnService,
                _characterRangeSpawnService,
                _characterMeleePool,
                _characterRangePool);
        }
    }
}