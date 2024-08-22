using System.Collections.Generic;
using Sources.BoundedContexts.Upgrades.Domain.Configs;

namespace Sources.BoundedContexts.Upgrades.Domain.Data
{
    public class RuntimeUpgradeConfig
    {
        public RuntimeUpgradeConfig(UpgradeConfig config)
        {
            Id = config.Id;
            List<RuntimeUpgradeLevel> levels = new List<RuntimeUpgradeLevel>();
            config.Levels.ForEach(level => levels.Add(new RuntimeUpgradeLevel(level)));
            Levels = levels;
        }

        public string Id { get; set; }
        public List<RuntimeUpgradeLevel> Levels { get; set; }
    }
}