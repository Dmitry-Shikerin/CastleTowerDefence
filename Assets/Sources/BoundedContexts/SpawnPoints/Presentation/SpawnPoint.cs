using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using Sources.BoundedContexts.SpawnPoints.PresentationInterfaces;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.BoundedContexts.SpawnPoints.Presentation
{
    public class SpawnPoint : View, ISpawnPoint
    {
        [SerializeField] private SpawnPointType _type;
        
        public SpawnPointType Type => _type;
        public Vector3 Position => transform.position;
    }
}