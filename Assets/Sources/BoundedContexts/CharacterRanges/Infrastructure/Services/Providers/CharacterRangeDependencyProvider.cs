using System;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers
{
    public class CharacterRangeDependencyProvider : MonoBehaviour
    {
        public CharacterRange CharacterRange { get; private set; }
        public ICharacterRangeView View { get; private set; }
        public ICharacterRangeAnimation Animation { get; private set; }
        
        public void Construct(
            CharacterRange characterRange, 
            ICharacterRangeView characterRangeView,
            ICharacterRangeAnimation rangeAnimation)
        {
            CharacterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            View = characterRangeView ?? throw new ArgumentNullException(nameof(characterRangeView));
            Animation = rangeAnimation ?? throw new ArgumentNullException(nameof(rangeAnimation));
        }   
    }
}