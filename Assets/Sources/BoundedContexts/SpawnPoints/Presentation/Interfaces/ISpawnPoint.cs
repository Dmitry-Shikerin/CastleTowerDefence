using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using UnityEngine;

namespace Sources.BoundedContexts.SpawnPoints.PresentationInterfaces
{
    public interface ISpawnPoint
    {
        SpawnPointType Type { get; }
        Vector3 Position { get; }
    }
}