using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.CharacterSpawnAbilities.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Implementation.Types;

namespace Sources.BoundedContexts.SpawnPoints.Extensions
{
    public static class SpawnPointExtension
    {
        public static List<CharacterSpawnPoint> GetSpawnPoints(
            this CharacterSpawnAbilityView gameObject, SpawnPointType type)
        {
            return gameObject.GetComponentsInChildren<CharacterSpawnPoint>()
                .Where(spawnPoint => spawnPoint.Type == type)
                .ToList();
        }
        
        public static List<EnemySpawnPoint> GetSpawnPoints(this EnemySpawnerView gameObject, SpawnPointType type)
        {
            return gameObject.GetComponentsInChildren<EnemySpawnPoint>()
                .Where(spawnPoint => spawnPoint.Type == type)
                .ToList();
        }
        
        public static void ValidateSpawnPoints(this 
            List<CharacterSpawnPoint> spawnPoints, 
            SpawnPointType spawnPointType, 
            SelfValidationResult result)
        {
            if (spawnPoints.Count == 0)
                result.AddError($"SpawnPoint type {spawnPointType} contains no SpawnPoints");
            
            foreach (CharacterSpawnPoint spawnPoint in spawnPoints)
            {
                if(spawnPoint.Type != spawnPointType)
                    result.AddError($"SpawnPoint {spawnPoint.gameObject.name} type isn't {spawnPoint.Type}");
                
                if(spawnPoint == null)
                    result.AddError($"SpawnPoint {spawnPoint.gameObject.name} not found");
            }
        } 
        public static void ValidateSpawnPoints(this 
            List<EnemySpawnPoint> spawnPoints, 
            SpawnPointType spawnPointType, 
            SelfValidationResult result)
        {
            if (spawnPoints.Count == 0)
                result.AddError($"SpawnPoint type {spawnPointType} contains no SpawnPoints");
            
            foreach (EnemySpawnPoint spawnPoint in spawnPoints)
            {
                if(spawnPoint.Type != spawnPointType)
                    result.AddError($"SpawnPoint {spawnPoint.gameObject.name} type isn't {spawnPoint.Type}");
                
                if(spawnPoint == null)
                    result.AddError($"SpawnPoint {spawnPoint.gameObject.name} not found");
            }
        }
    }
}