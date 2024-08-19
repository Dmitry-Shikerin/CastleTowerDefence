using System;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Prefabs;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Implementation
{
    public class CharacterRangeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly CharacterRangeDependencyProviderFactory _providerFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;

        public CharacterRangeViewFactory(
            IPoolManager poolManager,
            CharacterHealthViewFactory characterHealthViewFactory,
            CharacterRangeDependencyProviderFactory providerFactory,
            HealthBarViewFactory healthBarViewFactory)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _healthBarViewFactory = healthBarViewFactory ?? throw new ArgumentNullException(nameof(healthBarViewFactory));
        }
        
        public ICharacterRangeView Create(Upgrade characterHealthUpgrade, Vector3 position)
        {
            CharacterRange characterRange = new CharacterRange(
                new CharacterHealths.Domain.CharacterHealth(characterHealthUpgrade));
            
            CharacterRangeView view = _poolManager.Get<CharacterRangeView>(PrefabPath.CharacterRangeView);
            
            _characterHealthViewFactory.Create(characterRange.CharacterHealth, view.HealthView);
            _healthBarViewFactory.Create(characterRange.CharacterHealth, view.HealthBarView);
            _providerFactory.Create(characterRange,view);
            
            view.StartFsm();
            
            view.SetPosition(position);
            view.Show();
            
            return view;
        }
    }
}