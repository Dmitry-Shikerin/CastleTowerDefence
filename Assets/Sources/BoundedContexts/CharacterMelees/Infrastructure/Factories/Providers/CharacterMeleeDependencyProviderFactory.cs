using System;
using Sources.BoundedContexts.CharacterMelees.Domain;
using Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Factories.Providers
{
    public class CharacterMeleeDependencyProviderFactory
    {
        private readonly IOverlapService _overlapService;
        private readonly ICharacterRotationService _characterRotationService;

        public CharacterMeleeDependencyProviderFactory(
            IOverlapService overlapService,
            ICharacterRotationService characterRotationService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _characterRotationService = characterRotationService ?? 
                                        throw new ArgumentNullException(nameof(characterRotationService));
        }

        public CharacterMeleeDependencyProvider Create(
            CharacterMelee characterMelee, 
            ICharacterMeleeView meleeView)
        {
            CharacterMeleeDependencyProvider provider = meleeView.Provider;
            provider.Construct(
                characterMelee,
                meleeView, 
                meleeView.MeleeAnimation,
                _overlapService,
                _characterRotationService);

            return provider;
        }
    }
}