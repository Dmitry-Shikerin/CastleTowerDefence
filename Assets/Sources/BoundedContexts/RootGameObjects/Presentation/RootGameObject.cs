using Sirenix.OdinInspector;
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
        
        public CharacterSpawnerView CharacterSpawnerView => _characterSpawnerView;
        public EnemySpawnerView EnemySpawnerView => _enemySpawnerView;
    }
}