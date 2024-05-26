using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
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
        [FoldoutGroup("SpawnPoints")] [ChildGameObjectsOnly]
        [SerializeField] private List<SpawnPoint> _enemySpawnPoints;
        
        public List<SpawnPoint> CharactersMeleeSpawnPoints => _charactersMeleeSpawnPoints;
        public List<SpawnPoint> CharactersRangedSpawnPoints => _charactersRangedSpawnPoints;
        public List<SpawnPoint> EnemySpawnPoints => _enemySpawnPoints;
        
        public void Validate(SelfValidationResult result)
        {
            ValidateSpawnPoints(SpawnPointType.CharacterMelee, _charactersMeleeSpawnPoints, result);
            ValidateSpawnPoints(SpawnPointType.CharacterRanged, _charactersRangedSpawnPoints, result);
            ValidateSpawnPoints(SpawnPointType.Enemy, _enemySpawnPoints, result);
        }
        
        private void ValidateSpawnPoints(
            SpawnPointType spawnPointType, 
            List<SpawnPoint> spawnPoints, 
            SelfValidationResult result)
        {
            if (spawnPoints.Count == 0)
                result.AddError($"SpawnPoint type {spawnPointType} contains no SpawnPoints");
            
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if(spawnPoint.Type != spawnPointType)
                    result.AddError($"SpawnPoint {spawnPoint.gameObject.name} type isn't {spawnPoint.Type}");
                
                if(spawnPoint == null)
                    result.AddError($"SpawnPoint {spawnPoint.gameObject.name} not found");
            }
        }
        
        [FoldoutGroup("SpawnPoints")]
        [Button]
        private void AddCharacterMeleeSpawnPoints() =>
            _charactersMeleeSpawnPoints = GetSpawnPoints(SpawnPointType.CharacterMelee);
        
        [FoldoutGroup("SpawnPoints")]
        [Button]
        private void AddCharacterRangeSpawnPoints() =>
            _charactersRangedSpawnPoints = GetSpawnPoints(SpawnPointType.CharacterRanged);
        
        [FoldoutGroup("SpawnPoints")]
        [Button]
        private void AddEnemySpawnPoints() =>
            _enemySpawnPoints = GetSpawnPoints(SpawnPointType.Enemy);

        private List<SpawnPoint> GetSpawnPoints(SpawnPointType type)
        {
            return GetComponentsInChildren<SpawnPoint>()
                .Where(spawnPoint => spawnPoint.Type == type)
                .ToList();
        }
    }
}