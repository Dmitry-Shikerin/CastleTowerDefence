using System;
using Sources.BoundedContexts.CharacterMelees.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.InfrastructureInterfaces.Services.Overlaps;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers
{
    public class CharacterMeleeDependencyProvider : MonoBehaviour
    {
        public ICharacterMeleeView View { get; private set; }
        public ICharacterMeleeAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public ICharacterRotationService CharacterRotationService { get; private set; }

        public void Construct(
            ICharacterMeleeView view, 
            ICharacterMeleeAnimation meleeAnimation,
            IOverlapService overlapService,
            ICharacterRotationService characterRotationService)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
            Animation = meleeAnimation ?? throw new ArgumentNullException(nameof(meleeAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            CharacterRotationService = characterRotationService ?? 
                                       throw new ArgumentNullException(nameof(characterRotationService));
        }
    }
}