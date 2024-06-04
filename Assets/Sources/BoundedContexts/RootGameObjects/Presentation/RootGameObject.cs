using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.RootGameObjects.Presentation
{
    public class RootGameObject : MonoBehaviour
    {
        [FormerlySerializedAs("_characterSpawnerView")]
        [FoldoutGroup("Spawners")]
        [Required] [SerializeField] private CharacterSpawnAbilityView characterSpawnAbilityView;
        [FoldoutGroup("Spawners")]
        [Required] [SerializeField] private EnemySpawnerView _enemySpawnerView;

        [FoldoutGroup("Bunkers")] [Required] 
        [SerializeField] private BunkerView _bunkerView;
        
        public CharacterSpawnAbilityView CharacterSpawnAbilityView => characterSpawnAbilityView;
        public EnemySpawnerView EnemySpawnerView => _enemySpawnerView;

        public BunkerView BunkerView => _bunkerView;
    }
}