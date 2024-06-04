using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.NukeAbilities.Presentation.Implementation;
using UnityEngine;

namespace Sources.BoundedContexts.RootGameObjects.Presentation
{
    public class RootGameObject : MonoBehaviour
    {
        [FoldoutGroup("Spawners")]
        [Required] [SerializeField] private EnemySpawnerView _enemySpawnerView;

        [FoldoutGroup("Bunkers")] 
        [Required] [SerializeField] private BunkerView _bunkerView;
        
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private CharacterSpawnAbilityView characterSpawnAbilityView;
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private NukeAbilityView nukeAbilityView;
        
        public EnemySpawnerView EnemySpawnerView => _enemySpawnerView;

        public BunkerView BunkerView => _bunkerView;
        
        public CharacterSpawnAbilityView CharacterSpawnAbilityView => characterSpawnAbilityView;
        public NukeAbilityView NukeAbilityView => nukeAbilityView;
    }
}