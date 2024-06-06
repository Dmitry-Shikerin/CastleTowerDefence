using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using UnityEngine;

namespace Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces
{
    public interface ISpawnPoint
    {
        SpawnPointType Type { get; }
        Vector3 Position { get; }
    }
}