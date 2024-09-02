using System;
using Cysharp.Threading.Tasks;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterSpawnAbilities.Domain;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.GameServices.Repositories.Services.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Controllers
{
    public class CharacterSpawnAbilityPresenter : PresenterBase
    {
        private readonly CharacterSpawnAbility _characterSpawnAbility;
        private readonly Upgrade _characterHealthUpgrade;
        private readonly IPoolManager _poolManager;
        private readonly ICharacterSpawnAbilityView _view;
        private readonly CharacterMeleeViewFactory _characterMeleeViewFactory;
        private readonly CharacterRangeViewFactory _characterRangeViewFactory;

        public CharacterSpawnAbilityPresenter(
            IPoolManager poolManager, 
            IEntityRepository entityRepository,
            ICharacterSpawnAbilityView view,
            CharacterMeleeViewFactory characterMeleeViewFactory,
            CharacterRangeViewFactory characterRangeViewFactory)
        {
            if (entityRepository == null)
                throw new ArgumentNullException(nameof(entityRepository));
            
            _characterSpawnAbility = entityRepository.Get<CharacterSpawnAbility>(ModelId.SpawnAbility);
            _characterHealthUpgrade = entityRepository.Get<Upgrade>(ModelId.HealthUpgrade);
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _characterMeleeViewFactory = characterMeleeViewFactory ?? throw new ArgumentNullException(nameof(characterMeleeViewFactory));
            _characterRangeViewFactory = characterRangeViewFactory ?? throw new ArgumentNullException(nameof(characterRangeViewFactory));
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
            _view.HealAnimator.Play();
            DespawnMelee();
            DespawnRange();
            await UniTask.Yield();
            SpawnMelee();
            SpawnRange();
        }

        private void DespawnMelee()
        {
            foreach (CharacterMeleeView meleeView in _poolManager.GetPool<CharacterMeleeView>().Collection)
            {
                if (_poolManager.Contains(meleeView))
                    continue;
                
                meleeView.Destroy();
            }
        }
        
        private void DespawnRange()
        {
            foreach (CharacterRangeView rangeView in _poolManager.GetPool<CharacterRangeView>().Collection)
            {
                if (_poolManager.Contains(rangeView))
                    continue;
                
                rangeView.Destroy();
            }
        }

        private void SpawnMelee()
        {
            foreach (ICharacterSpawnPoint spawnPoint in _view.MeleeSpawnPoints)
            {
                ICharacterMeleeView view = _characterMeleeViewFactory.Create(
                    _characterHealthUpgrade,
                    spawnPoint.Position);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }

        private void SpawnRange()
        {
            foreach (var spawnPoint in _view.RangeSpawnPoints)
            {
                ICharacterRangeView view = _characterRangeViewFactory.Create(
                    _characterHealthUpgrade,
                    spawnPoint.Position);
                view.SetCharacterSpawnPoint(spawnPoint);
            }
        }
    }
}