using System;
using Sources.BoundedContexts.CharacterHealth.Infrastructure.Factories.Views;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Presentation;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Interfaces;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Views;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.BoundedContexts.Prefabs;
using Sources.Domain.Models.Constants;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Views.Implementation
{
    public class CharacterRangeViewFactory : PoolableObjectFactory<CharacterRangeView>, ICharacterRangeViewFactory
    {
        private readonly CharacterHealthViewFactory _characterHealthViewFactory;
        private readonly CharacterRangeDependencyProviderFactory _providerFactory;
        private readonly HealthBarViewFactory _healthBarViewFactory;

        public CharacterRangeViewFactory(
            CharacterHealthViewFactory characterHealthViewFactory,
            CharacterRangeDependencyProviderFactory providerFactory,
            HealthBarViewFactory healthBarViewFactory,
            IObjectPool<CharacterRangeView> pool) 
            : base(pool)
        {
            _characterHealthViewFactory = characterHealthViewFactory ?? 
                                          throw new ArgumentNullException(nameof(characterHealthViewFactory));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _healthBarViewFactory = healthBarViewFactory ?? throw new ArgumentNullException(nameof(healthBarViewFactory));
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
            _healthBarViewFactory.Create(characterRange.CharacterHealth, view.HealthBarView);
            
            return view;
        }
    }
}