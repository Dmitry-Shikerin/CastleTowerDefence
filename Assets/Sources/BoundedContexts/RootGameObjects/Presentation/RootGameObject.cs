using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.SpawnPoints.Extensions;
using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using UnityEngine;

namespace Sources.BoundedContexts.RootGameObjects.Presentation
{
    public class RootGameObject : MonoBehaviour, ISelfValidator
    {
        [FoldoutGroup("SpawnPoints")] [ChildGameObjectsOnly]
        [SerializeField] private List<SpawnPoint> _charactersMeleeSpawnPoints;
        [FoldoutGroup("SpawnPoints")] [ChildGameObjectsOnly]
        [SerializeField] private List<SpawnPoint> _charactersRangedSpawnPoints;

        
        public List<SpawnPoint> CharactersMeleeSpawnPoints => _charactersMeleeSpawnPoints;
        public List<SpawnPoint> CharactersRangedSpawnPoints => _charactersRangedSpawnPoints;
        
        public void Validate(SelfValidationResult result)
        {
            _charactersMeleeSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterMelee,  result);
            _charactersRangedSpawnPoints.ValidateSpawnPoints(SpawnPointType.CharacterRanged,  result);
        }

        
        [FoldoutGroup("SpawnPoints")]
        [Button]
        private void AddCharacterMeleeSpawnPoints() =>
            _charactersMeleeSpawnPoints = gameObject.GetSpawnPoints(SpawnPointType.CharacterMelee);
        
        [FoldoutGroup("SpawnPoints")]
        [Button]
        private void AddCharacterRangeSpawnPoints() =>
            _charactersRangedSpawnPoints = gameObject.GetSpawnPoints(SpawnPointType.CharacterRanged);
    }
}