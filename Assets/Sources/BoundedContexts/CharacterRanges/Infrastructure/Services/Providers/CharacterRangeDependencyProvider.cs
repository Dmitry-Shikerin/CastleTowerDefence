using System;
using Sources.BoundedContexts.CharacterRanges.Domain;
using Sources.BoundedContexts.CharacterRanges.Presentation.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterRanges.Infrastructure.Services.Providers
{
    public class CharacterRangeDependencyProvider : MonoBehaviour
    {
        public CharacterRange CharacterRange { get; private set; }
        public ICharacterRangeView CharacterRangeView { get; private set; }
        
        public void Construct(CharacterRange characterRange, ICharacterRangeView characterRangeView)
        {
            CharacterRange = characterRange ?? throw new ArgumentNullException(nameof(characterRange));
            CharacterRangeView = characterRangeView ?? throw new ArgumentNullException(nameof(characterRangeView));
        }   
    }
}