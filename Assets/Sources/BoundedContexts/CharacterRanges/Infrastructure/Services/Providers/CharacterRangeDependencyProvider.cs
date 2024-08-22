using System;
using System.ComponentModel;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using Sources.BoundedContexts.CharacterRotations.Services.Interfaces;
using Sources.Frameworks.GameServices.Overlaps.Interfaces;
using UnityEngine;
using Zenject;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers
{
    public class CharacterRangeDependencyProvider : MonoBehaviour
    {
        public CharacterRange CharacterRange { get; private set; }
        public ICharacterRangeView View { get; private set; }
        public ICharacterRangeAnimation Animation { get; private set; }
        public IOverlapService OverlapService { get; private set; }
        public ICharacterRotationService CharacterRotationService { get; private set; }
        public DiContainer Container { get; private set; }
        
        public void Construct(
            CharacterRange characterRange, 
            ICharacterRangeView characterRangeView,
            ICharacterRangeAnimation rangeAnimation,
            IOverlapService overlapService,
            ICharacterRotationService characterRotationService,
            DiContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
            CharacterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            View = characterRangeView ?? throw new ArgumentNullException(nameof(characterRangeView));
            Animation = rangeAnimation ?? throw new ArgumentNullException(nameof(rangeAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            CharacterRotationService = characterRotationService ?? 
                                       throw new ArgumentNullException(nameof(characterRotationService));
        }   
    }
}