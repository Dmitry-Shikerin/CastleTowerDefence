using System;
using Sources.BoundedContexts.CharacterHealths.Domain;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.Utils.Injects;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views
{
    public class CharacterRangeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly DiContainer _diContainer;

        public CharacterRangeViewFactory(
            IPoolManager poolManager,
            CharacterHealthViewFactory characterHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            DiContainer diContainer)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? throw new ArgumentNullException(nameof(healthBarViewFactory));
            _diContainer = diContainer ?? throw new ArgumentNullException(nameof(diContainer));
        }
        
        public ICharacterRangeView Create(Upgrade characterHealthUpgrade, Vector3 position)
        {
            Character characterRange = new Character(
                new CharacterHealth(characterHealthUpgrade));
            
            CharacterRangeView view = _poolManager.Get<CharacterRangeView>();
            
            _characterHealthViewFactory.Create(characterRange.CharacterHealth, view.HealthView);
            _healthBarViewFactory.Create(characterRange.CharacterHealth, view.HealthBarView);
            
            view.FsmOwner.InjectFsm(_diContainer);
            view.FsmOwner.ConstructFsm(characterRange, view);
            _diContainer.Inject(view.LookAtCamera);
            view.StartFsm();
            
            view.SetPosition(position);
            view.Show();
            
            return view;
        }
    }
}