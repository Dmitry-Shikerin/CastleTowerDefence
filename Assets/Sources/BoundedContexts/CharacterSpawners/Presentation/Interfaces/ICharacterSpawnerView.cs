using System.Collections.Generic;
using Sources.BoundedContexts.SpawnPoints.PresentationInterfaces;

namespace Sources.BoundedContexts.CharacterSpawners.Presentation.Interfaces
{
    public interface ICharacterSpawnerView
    {
        IReadOnlyList<ISpawnPoint> MeleeSpawnPoints { get; }
        IReadOnlyList<ISpawnPoint> RangeSpawnPoints { get; }
    }
}