using System;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Prefabs;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Implementation
{
    public class CharacterMeleeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly CharacterMeleeDependencyProviderFactory _providerFactory;
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;

        public CharacterMeleeViewFactory(
            IPoolManager poolManager,
            CharacterMeleeDependencyProviderFactory providerFactory,
            CharacterHealthViewFactory characterHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
        }
        
        public ICharacterMeleeView Create(Upgrade characterHealthUpgrade, Vector3 position)
        {
            CharacterMelee characterMelee = new CharacterMelee(
                new CharacterHealths.Domain.CharacterHealth(characterHealthUpgrade));
            
            CharacterMeleeView view = _poolManager.Get<CharacterMeleeView>(PrefabPath.CharacterMeleeView);

            _providerFactory.Create(characterMelee, view);
            _characterHealthViewFactory.Create(characterMelee.CharacterHealth, view.HealthView);
            _healthBarViewFactory.Create(characterMelee.CharacterHealth, view.HealthBarView);
            
            view.StartBehaviour();
            
            view.SetPosition(position);
            view.Show();
            
            return view;
        }
    }
}