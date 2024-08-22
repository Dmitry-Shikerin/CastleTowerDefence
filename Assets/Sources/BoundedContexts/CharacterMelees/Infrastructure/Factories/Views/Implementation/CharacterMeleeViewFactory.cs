using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.Utils.Injects;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Implementation
{
    public class CharacterMeleeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly CharacterMeleeDependencyProviderFactory _providerFactory;
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly DiContainer _container;

        public CharacterMeleeViewFactory(
            IPoolManager poolManager,
            CharacterMeleeDependencyProviderFactory providerFactory,
            CharacterHealthViewFactory characterHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            DiContainer container)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        
        public ICharacterMeleeView Create(Upgrade characterHealthUpgrade, Vector3 position)
        {
            CharacterMelee characterMelee = new CharacterMelee(
                new CharacterHealths.Domain.CharacterHealth(characterHealthUpgrade));
            
            CharacterMeleeView view = _poolManager.Get<CharacterMeleeView>();

            _providerFactory.Create(characterMelee, view);
            _characterHealthViewFactory.Create(characterMelee.CharacterHealth, view.HealthView);
            _healthBarViewFactory.Create(characterMelee.CharacterHealth, view.HealthBarView);
            
            view.FsmOwner.ConstructFsm(characterMelee, view);
            view.FsmOwner.InjectFsm(_container);
            view.StartBehaviour();
            
            view.SetPosition(position);
            view.Show();
            
            return view;
        }
    }
}