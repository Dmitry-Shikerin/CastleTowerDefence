using Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects;
using UnityEngine;

namespace Sources.BoundedContexts.EnemySpawners.Domain.Configs
{
    [CreateAssetMenu(
        fileName = "EnemySpawnStrategyCollector", 
        menuName = "Configs/EnemySpawnStrategyCollector", order = 51)]
    public class EnemySpawnStrategyCollector : ConfigCollector<EnemySpawnStrategy>
    {
    }
}