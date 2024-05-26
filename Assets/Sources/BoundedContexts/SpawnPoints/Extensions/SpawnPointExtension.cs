using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sources.BoundedContexts.SpawnPoints.Presentation;
using Sources.BoundedContexts.SpawnPoints.Presentation.Types;
using UnityEngine;

namespace Sources.BoundedContexts.SpawnPoints.Extensions
{
    public static class SpawnPointExtension
    {
        public static List<SpawnPoint> GetSpawnPoints(this GameObject gameObject, SpawnPointType type)
        {
            return gameObject.GetComponentsInChildren<SpawnPoint>()
                .Where(spawnPoint => spawnPoint.Type == type)
                .ToList();
        }
        
        public static void ValidateSpawnPoints(this 
            List<SpawnPoint> spawnPoints, 
            SpawnPointType spawnPointType, 
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
    }
}