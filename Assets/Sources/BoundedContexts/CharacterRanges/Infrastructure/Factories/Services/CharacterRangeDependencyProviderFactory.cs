using System;
using JetBrains.Annotations;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using Zenject;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services
{
    public class CharacterRangeDependencyProviderFactory
    {
        private readonly IOverlapService _overlapService;
        private readonly ICharacterRotationService _characterRotationService;
        private readonly DiContainer _diContainer;

        public CharacterRangeDependencyProviderFactory(
            IOverlapService overlapService,
            ICharacterRotationService characterRotationService,
            DiContainer diContainer)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _characterRotationService = characterRotationService ?? 
                                        throw new ArgumentNullException(nameof(characterRotationService));
            _diContainer = diContainer ?? throw new ArgumentNullException(nameof(diContainer));
        }

        public CharacterRangeDependencyProvider Create(
            CharacterRange characterRange, 
            CharacterRangeView view)
        {
            view.Provider.Construct(
                characterRange, 
                view,
                view.RangeAnimation,
                _overlapService,
                _characterRotationService,
                _diContainer);
            
            return view.Provider;
        }
    }
}