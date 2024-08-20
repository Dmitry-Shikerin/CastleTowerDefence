using System.Collections.Generic;
using Doozy.Runtime.Reactor.Animators;
using Doozy.Runtime.UIManager.Components;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces
{
    public interface ICharacterSpawnAbilityView
    {
        UIAnimator HealAnimator { get; }
        IReadOnlyList<ICharacterSpawnPoint> MeleeSpawnPoints { get; }
        IReadOnlyList<ICharacterSpawnPoint> RangeSpawnPoints { get; }
    
    }
}