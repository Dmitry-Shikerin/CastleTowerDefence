using UnityEngine;

namespace Sources.Frameworks.GameServices.ConfigCollectors.Tests
{
    [CreateAssetMenu(fileName = "TestCollectorContainer", menuName = "Configs/TestCollectorContainer")]
    public class TestCollectorContainer : CollectorContainer<TestConfigCollector, TestConfig>
    {
    }
}