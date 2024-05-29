using System;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Providers;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterMelees.Presentation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Implementation;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.Services.ObjectPools.Generic;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Views.Implementation
{
    public class CharacterMeleeViewFactory : PoolableObjectFactory<CharacterMeleeView>, ICharacterMeleeViewFactory
    {
        private readonly CharacterMeleeDependencyProviderFactory _providerFactory;
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;

        public CharacterMeleeViewFactory(
            IObjectPool<CharacterMeleeView> meleePool,
            CharacterMeleeDependencyProviderFactory providerFactory,
            CharacterHealthViewFactory characterHealthViewFactory) 
            : base(meleePool)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _characterHealthViewFactory = characterHealthViewFactory ?? throw new ArgumentNullException(nameof(characterHealthViewFactory));
        }
        
        public ICharacterMeleeView Create(CharacterMelee characterMelee)
        {
            CharacterMeleeView view = CreateView(PrefabPath.CharacterMeleeView);
            
            return Create(characterMelee, view);
        }

        public ICharacterMeleeView Create(CharacterMelee characterMelee, CharacterMeleeView view)
        {
            _providerFactory.Create(view);
            view.FSMOwner.StartBehaviour();
            
            _characterHealthViewFactory.Create(characterMelee.CharacterHealth, view.HealthView);
            
            return view;
        }
    }
}