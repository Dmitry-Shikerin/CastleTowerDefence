using Sirenix.OdinInspector;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Implementation;
using Sources.BoundedContexts.NukeAbilities.Presentation.Implementation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.BoundedContexts.RootGameObjects.Presentation
{
    public class RootGameObject : MonoBehaviour
    {
        [FoldoutGroup("Spawners")]
        [Required] [SerializeField] private EnemySpawnerView _enemySpawnerView;

        [FoldoutGroup("Bunkers")] 
        [Required] [SerializeField] private BunkerView _bunkerView;
        
        [FormerlySerializedAs("characterSpawnAbilityView")]
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private CharacterSpawnAbilityView _characterSpawnAbilityView;
        [FormerlySerializedAs("nukeAbilityView")]
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private NukeAbilityView _nukeAbilityView;
        [FoldoutGroup("Abilities")]
        [Required] [SerializeField] private FlamethrowerAbilityView _flamethrowerAbilityView;
        
        public EnemySpawnerView EnemySpawnerView => _enemySpawnerView;

        public BunkerView BunkerView => _bunkerView;
        
        public CharacterSpawnAbilityView CharacterSpawnAbilityView => _characterSpawnAbilityView;
        public NukeAbilityView NukeAbilityView => _nukeAbilityView;
        public FlamethrowerAbilityView FlamethrowerAbilityView => _flamethrowerAbilityView;
    }
}