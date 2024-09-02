using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces
{
    public interface ICharacterSpawnAbilityView
    {
        UIAnimator HealAnimator { get; }
        IReadOnlyList<ICharacterSpawnPoint> MeleeSpawnPoints { get; }
        IReadOnlyList<ICharacterSpawnPoint> RangeSpawnPoints { get; }
    
    }
}