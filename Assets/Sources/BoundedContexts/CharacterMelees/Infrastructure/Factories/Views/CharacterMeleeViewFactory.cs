using System;
using Sources.BoundedContexts.CharacterHealths.Domain;
using Sources.BoundedContexts.CharacterHealths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.Characters.Domain;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Managers;
using Sources.Frameworks.Utils.Injects;
using Sources.Frameworks.Utils.Reflections;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views
{
    public class CharacterMeleeViewFactory
    {
        private readonly IPoolManager _poolManager;
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;
        private readonly DiContainer _container;

        public CharacterMeleeViewFactory(
            IPoolManager poolManager,
            CharacterHealthViewFactory characterHealthViewFactory,
            HealthBarViewFactory healthBarViewFactory,
            DiContainer container)
        {
            _poolManager = poolManager ?? throw new ArgumentNullException(nameof(poolManager));
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _healthBarViewFactory = healthBarViewFactory ?? 
                                    throw new ArgumentNullException(nameof(healthBarViewFactory));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        
        public ICharacterMeleeView Create(Upgrade characterHealthUpgrade, Vector3 position)
        {
            Character characterMelee = new Character(
                new CharacterHealth(characterHealthUpgrade));
            
            CharacterMeleeView view = _poolManager.Get<CharacterMeleeView>();

            _characterHealthViewFactory.Create(characterMelee.CharacterHealth, view.HealthView);
            _healthBarViewFactory.Create(characterMelee.CharacterHealth, view.HealthBarView);
            
            view.FsmOwner.ConstructFsm(characterMelee, view);
            view.FsmOwner.InjectFsm(_container);
            view.StartFsm();
            
            view.SetPosition(position);
            view.Show();
            
            return view;
        }
    }
}