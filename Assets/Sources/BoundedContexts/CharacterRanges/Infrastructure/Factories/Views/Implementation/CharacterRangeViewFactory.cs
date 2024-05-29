using System;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Presentation;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.Services.ObjectPools.Generic;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Implementation
{
    public class CharacterRangeViewFactory : PoolableObjectFactory<CharacterRangeView>, ICharacterRangeViewFactory
    {
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly CharacterRangeDependencyProviderFactory _providerFactory;

        public CharacterRangeViewFactory(
            CharacterHealthViewFactory characterHealthViewFactory,
            CharacterRangeDependencyProviderFactory providerFactory,
            IObjectPool<CharacterRangeView> pool) 
            : base(pool)
        {
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
        }
        
        public ICharacterRangeView Create(CharacterRange characterRange)
        {
            CharacterRangeView view = CreateView(PrefabPath.CharacterRangeView);
            
            return Create(characterRange, view);
        }

        public ICharacterRangeView Create(CharacterRange characterRange, CharacterRangeView view)
        {
            _providerFactory.Create(characterRange,view);
            view.FSMOwner.StartBehaviour();
            
            _characterHealthViewFactory.Create(characterRange.CharacterHealth, view.HealthView);
            
            return view;
        }
    }
}