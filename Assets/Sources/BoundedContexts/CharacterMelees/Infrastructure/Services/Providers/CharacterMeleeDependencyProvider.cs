using System;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using Sources.InfrastructureInterfaces.Services.Overlaps;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers
{
    public class CharacterMeleeDependencyProvider : MonoBehaviour
    {
        public ICharacterMeleeView MeleeView { get; private set; }
        public ICharacterMeleeAnimation MeleeAnimation { get; private set; }
        public IOverlapService OverlapService { get; private set; }

        public void Construct(
            ICharacterMeleeView meleeView, 
            ICharacterMeleeAnimation meleeAnimation,
            IOverlapService overlapService)
        {
            MeleeView = meleeView ?? throw new ArgumentNullException(nameof(meleeView));
            MeleeAnimation = meleeAnimation ?? throw new ArgumentNullException(nameof(meleeAnimation));
            OverlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
        }
    }
}