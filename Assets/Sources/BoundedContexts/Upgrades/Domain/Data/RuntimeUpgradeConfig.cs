using System.Collections.Generic;

namespace Sources.BoundedContexts.Upgrades.Domain.Data
{
    public class RuntimeUpgradeConfig
    {
        public string Id { get; set; }
        public List<RuntimeUpgradeLevel> Levels { get; set; }
    }
}