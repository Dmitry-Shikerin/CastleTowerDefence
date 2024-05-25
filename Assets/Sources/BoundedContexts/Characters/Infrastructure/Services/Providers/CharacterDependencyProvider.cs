using System;
using Sources.BoundedContexts.Characters.PresentationInterfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Characters.Infrastructure.Services.Providers
{
    public class CharacterDependencyProvider : MonoBehaviour
    {
        public ICharacterView View { get; private set; }
        public ICharacterAnimation Animation { get; private set; }

        public void Construct(ICharacterView view, ICharacterAnimation animation)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
            Animation = animation ?? throw new ArgumentNullException(nameof(animation));
        }
    }
}