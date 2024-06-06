using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Interfaces
{
    public interface ICharacterSpawnAbilityView
    {
        public IReadOnlyList<ICharacterSpawnPoint> MeleeSpawnPoints { get; }
        public IReadOnlyList<ICharacterSpawnPoint> RangeSpawnPoints { get; }
    
    }
}