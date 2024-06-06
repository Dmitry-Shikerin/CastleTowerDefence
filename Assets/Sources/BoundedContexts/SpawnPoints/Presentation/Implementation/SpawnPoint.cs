using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;
using Sources.BoundedContexts.SpawnPoints.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using UnityEngine;

namespace Sources.BoundedContexts.SpawnPoints.Presentation.Implementation
{
    public class SpawnPoint : View, ISpawnPoint
    {
        [SerializeField] private SpawnPointType _type;

        public SpawnPointType Type => _type;
        public Vector3 Position => transform.position;
    }
}