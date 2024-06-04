using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Controllers
{
    public class CharacterSpawnAbilityPresenter : PresenterBase
    {
        private readonly CharacterSpawnAbility _characterSpawnAbility;
        private readonly ICharacterSpawnAbilityView _view;
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;
        private readonly ICharacterRangeSpawnService _characterRangeSpawnService;

        public CharacterSpawnAbilityPresenter(
            CharacterSpawnAbility characterSpawnAbility,
            ICharacterSpawnAbilityView view,
            ICharacterMeleeSpawnService characterMeleeSpawnService,
            ICharacterRangeSpawnService characterRangeSpawnService)
        {
            _characterSpawnAbility = characterSpawnAbility ?? throw new ArgumentNullException(nameof(characterSpawnAbility));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _characterMeleeSpawnService = characterMeleeSpawnService ??
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
            _characterRangeSpawnService = characterRangeSpawnService ??
                                          throw new ArgumentNullException(nameof(characterRangeSpawnService));
        }

        public override void Enable()
        {
            SpawnMelee();
            SpawnRange();
        }

        public override void Disable()
        {
        }
        
        private void SpawnMelee()
        {
            for (int i = 0; i < 2; i++)
            {
                var spawnPoint = _view.MeleeSpawnPoints[i];
                ICharacterMeleeView view = _characterMeleeSpawnService.Spawn(spawnPoint.Position);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }

        private void SpawnRange()
        {
            for (int i = 0; i < 2; i++)
            {
                var spawnPoint = _view.RangeSpawnPoints[i];
                ICharacterRangeView view = _characterRangeSpawnService.Spawn(spawnPoint.Position);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }
    }
}