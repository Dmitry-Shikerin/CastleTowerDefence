using System;
using Sources.BoundedContexts.CharacterMelees.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterMelees.Infrastructure.Services.Providers
{
    public class CharacterMeleeDependencyProvider : MonoBehaviour
    {
        public ICharacterMeleeView MeleeView { get; private set; }
        public ICharacterMeleeAnimation MeleeAnimation { get; private set; }

        public void Construct(ICharacterMeleeView meleeView, ICharacterMeleeAnimation meleeAnimation)
        {
            MeleeView = meleeView ?? throw new ArgumentNullException(nameof(meleeView));
            MeleeAnimation = meleeAnimation ?? throw new ArgumentNullException(nameof(meleeAnimation));
        }
    }
}