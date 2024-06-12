using System;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Spawners.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;
using Sources.InfrastructureInterfaces.Services.Repositories;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Controllers
{
    public class CharacterSpawnAbilityPresenter : PresenterBase
    {
        private readonly CharacterSpawnAbility _characterSpawnAbility;
        private readonly Upgrade _characterHealthUpgrade;
        private readonly ICharacterSpawnAbilityView _view;
        private readonly ICharacterMeleeSpawnService _characterMeleeSpawnService;
        private readonly ICharacterRangeSpawnService _characterRangeSpawnService;
        private readonly IObjectPool<CharacterMeleeView> _characterMeleePool;
        private readonly IObjectPool<CharacterRangeView> _characterRangePool;

        public CharacterSpawnAbilityPresenter(
            IEntityRepository entityRepository,
            ICharacterSpawnAbilityView view,
            ICharacterMeleeSpawnService characterMeleeSpawnService,
            ICharacterRangeSpawnService characterRangeSpawnService,
            IObjectPool<CharacterMeleeView> characterMeleePool,
            IObjectPool<CharacterRangeView> characterRangePool)
        {
            if (entityRepository == null)
                throw new ArgumentNullException(nameof(entityRepository));
            
            _characterSpawnAbility = entityRepository.Get<CharacterSpawnAbility>(ModelId.SpawnAbility);
            _characterHealthUpgrade = entityRepository.Get<Upgrade>(ModelId.HealthUpgrade);
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _characterMeleeSpawnService = characterMeleeSpawnService ??
                                          throw new ArgumentNullException(nameof(characterMeleeSpawnService));
            _characterRangeSpawnService = characterRangeSpawnService ??
                                          throw new ArgumentNullException(nameof(characterRangeSpawnService));
            _characterMeleePool = characterMeleePool ?? throw new ArgumentNullException(nameof(characterMeleePool));
            _characterRangePool = characterRangePool ?? throw new ArgumentNullException(nameof(characterRangePool));
        }

        public override void Enable()
        {
            SpawnMelee();
            SpawnRange();
            _characterSpawnAbility.AbilityApplied += OnAbilityApplied;
        }

        public override void Disable()
        {
            _characterSpawnAbility.AbilityApplied -= OnAbilityApplied;
        }

        private async void OnAbilityApplied()
        {
            DespawnMelee();
            DespawnRange();
            await UniTask.Yield();
            SpawnMelee();
            SpawnRange();
        }

        private void DespawnMelee()
        {
            foreach (CharacterMeleeView meleeView in _characterMeleePool.Collection)
            {
                if (_characterMeleePool.Contains(meleeView))
                    continue;
                
                meleeView.Destroy();
            }
        }
        
        private void DespawnRange()
        {
            foreach (CharacterRangeView rangeView in _characterRangePool.Collection)
            {
                if (_characterRangePool.Contains(rangeView))
                    continue;
                
                rangeView.Destroy();
            }
        }

        private void SpawnMelee()
        {
            foreach (ICharacterSpawnPoint spawnPoint in _view.MeleeSpawnPoints)
            {
                ICharacterMeleeView view = _characterMeleeSpawnService.Spawn(
                    spawnPoint.Position,
                    _characterHealthUpgrade);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }

        private void SpawnRange()
        {
            foreach (var spawnPoint in _view.RangeSpawnPoints)
            {
                ICharacterRangeView view = _characterRangeSpawnService.Spawn(
                    spawnPoint.Position,
                    _characterHealthUpgrade);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }
    }
}