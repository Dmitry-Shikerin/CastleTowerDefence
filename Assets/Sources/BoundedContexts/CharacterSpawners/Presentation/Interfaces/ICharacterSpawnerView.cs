using System.Collections.Generic;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;

namespace Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces
{
    public interface ICharacterSpawnerView
    {
        IReadOnlyList<ICharacterSpawnPoint> MeleeSpawnPoints { get; }
        IReadOnlyList<ICharacterSpawnPoint> RangeSpawnPoints { get; }
    }
}