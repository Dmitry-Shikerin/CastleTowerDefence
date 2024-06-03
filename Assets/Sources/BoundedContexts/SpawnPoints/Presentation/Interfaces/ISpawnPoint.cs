using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using UnityEngine;

namespace Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces
{
    public interface ISpawnPoint
    {
        bool IsEmpty { get; }
        SpawnPointType Type { get; }
        Vector3 Position { get; }

        void SetEmpty();
        void Fill();
    }
}