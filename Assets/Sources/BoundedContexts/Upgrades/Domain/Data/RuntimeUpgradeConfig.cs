using System.Collections.Generic;
using Sources.BoundedContexts.Upgrades.Domain.Configs;

namespace Sources.BoundedContexts.Upgrades.Domain.Data
{
    public class RuntimeUpgradeConfig
    {
        public string Id { get; set; }
        public List<RuntimeUpgradeLevel> Levels { get; set; }
    }
}