using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces
{
    public interface ICharacterSpawnAbilityView
    {
        UIButton SpawnButton { get; }
        IReadOnlyList<ICharacterSpawnPoint> MeleeSpawnPoints { get; }
        IReadOnlyList<ICharacterSpawnPoint> RangeSpawnPoints { get; }
    }
}