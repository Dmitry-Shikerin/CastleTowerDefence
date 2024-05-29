using System;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterRanges.Presentation.Implementation;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.InfrastructureInterfaces.Services.Overlaps;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Factories.Services
{
    public class CharacterRangeDependencyProviderFactory
    {
        private readonly IOverlapService _overlapService;
        private readonly ICharacterRotationService _characterRotationService;

        public CharacterRangeDependencyProviderFactory(
            IOverlapService overlapService,
            ICharacterRotationService characterRotationService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _characterRotationService = characterRotationService ?? 
                                        throw new ArgumentNullException(nameof(characterRotationService));
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
                _characterRotationService);
            
            return view.Provider;
        }
    }
}