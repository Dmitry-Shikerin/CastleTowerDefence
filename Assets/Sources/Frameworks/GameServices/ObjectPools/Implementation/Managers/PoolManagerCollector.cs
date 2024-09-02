using Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ObjectPools.Implementation.Managers
{
    [CreateAssetMenu(fileName = "PoolManagerCollector", menuName = "Configs/PoolManagerCollector", order = 51)]
    public class PoolManagerCollector : ConfigCollector<PoolManagerConfig>
    {
    }
}