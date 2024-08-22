using System;
using JetBrains.Annotations;
using NodeCanvas.StateMachines;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.Utils.Injects;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Implementation
{
    public class CharacterRangeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly CharacterRangeDependencyProviderFactory _providerFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly DiContainer _diContainer;

        public CharacterRangeViewFactory(
            IPoolManager poolManager,
            CharacterHealthViewFactory characterHealthViewFactory,
            CharacterRangeDependencyProviderFactory providerFactory,
            HealthBarViewFactory healthBarViewFactory,
            DiContainer diContainer)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _healthBarViewFactory = healthBarViewFactory ?? throw new ArgumentNullException(nameof(healthBarViewFactory));
            _diContainer = diContainer ?? throw new ArgumentNullException(nameof(diContainer));
        }
        
        public ICharacterRangeView Create(Upgrade characterHealthUpgrade, Vector3 position)
        {
            CharacterRange characterRange = new CharacterRange(
                new CharacterHealths.Domain.CharacterHealth(characterHealthUpgrade));
            
            CharacterRangeView view = _poolManager.Get<CharacterRangeView>();
            
            _characterHealthViewFactory.Create(characterRange.CharacterHealth, view.HealthView);
            _healthBarViewFactory.Create(characterRange.CharacterHealth, view.HealthBarView);
            _providerFactory.Create(characterRange,view);
            
            view.FsmOwner.InjectFsm(_diContainer);
            view.FsmOwner.ConstructFsm(characterRange, view);
            view.StartFsm();
            
            view.SetPosition(position);
            view.Show();
            
            return view;
        }
    }
}