using Sources.Frameworks.GameServices.ConfigCollectors.Domain.ScriptableObjects;
using UnityEngine;

namespace Sources.Frameworks.GameServices.ConfigCollectors.Tests.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TestCollectorContainer", menuName = "Configs/TestCollectorContainer")]
    public class TestCollectorContainer : CollectorContainer<TestConfigCollector, TestConfig>
    {
    }
}