using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawners.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.RootGameObjects.Presentation
{
    public class RootGameObject : MonoBehaviour
    {
        [FoldoutGroup("Spawners")]
        [Required] [SerializeField] private CharacterSpawnerView _characterSpawnerView;
        [FoldoutGroup("Spawners")]
        [Required] [SerializeField] private EnemySpawnerView _enemySpawnerView;

        [FoldoutGroup("Bunkers")] [Required] 
        [SerializeField] private BunkerView _bunkerView;
        
        public CharacterSpawnerView CharacterSpawnerView => _characterSpawnerView;
        public EnemySpawnerView EnemySpawnerView => _enemySpawnerView;

        public BunkerView BunkerView => _bunkerView;
    }
}