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

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Ifrastructure.Factories.Controllers
{
    public class CharacterSpawnAbilityPresenterFactory
    {
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;
        private readonly ICharacterRangeSpawnService _characterRangeSpawnService;
        private readonly IObjectPool<CharacterMeleeView> _characterMeleePool;
        private readonly IObjectPool<CharacterRangeView> _characterRangePool;

        public CharacterSpawnAbilityPresenterFactory(
            ICharacterMeleeSpawnService characterMeleeSpawnService,
            ICharacterRangeSpawnService characterRangeSpawnService,
            IObjectPool<CharacterMeleeView> characterMeleePool,
            IObjectPool<CharacterRangeView> characterRangePool)
        {
            _characterMeleeSpawnService = characterMeleeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
            _characterRangeSpawnService = characterRangeSpawnService ?? 
                                          throw new ArgumentNullException(nameof(characterRangeSpawnService));
            _characterMeleePool = characterMeleePool ?? throw new ArgumentNullException(nameof(characterMeleePool));
            _characterRangePool = characterRangePool ?? throw new ArgumentNullException(nameof(characterRangePool));
        }

        public CharacterSpawnAbilityPresenter Create(
            CharacterSpawnAbility characterSpawnAbility, 
            Upgrade characterHealthUpgrade,
            ICharacterSpawnAbilityView view)
        {
            return new CharacterSpawnAbilityPresenter(
                characterSpawnAbility,
                characterHealthUpgrade,
                view,
                _characterMeleeSpawnService,
                _characterRangeSpawnService,
                _characterMeleePool,
                _characterRangePool);
        }
    }
}