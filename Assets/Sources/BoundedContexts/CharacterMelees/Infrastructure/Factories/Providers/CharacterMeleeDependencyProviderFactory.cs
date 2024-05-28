using System;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.InfrastructureInterfaces.Services.Overlaps;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Providers
{
    public class CharacterMeleeDependencyProviderFactory
    {
        private readonly IOverlapService _overlapService;

        public CharacterMeleeDependencyProviderFactory(IOverlapService overlapService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
        }

        public CharacterMeleeDependencyProvider Create(ICharacterMeleeView meleeView)
        {
            CharacterMeleeDependencyProvider provider = meleeView.Provider;
            provider.Construct(
                meleeView, 
                meleeView.MeleeAnimation,
                _overlapService);

            return provider;
        }
    }
}