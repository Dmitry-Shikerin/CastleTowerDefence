using Sources.BoundedContexts.Upgrades.Domain.Configs;

namespace Sources.BoundedContexts.Upgrades.Domain.Data
{
    public class RuntimeUpgradeLevel
    {
        public RuntimeUpgradeLevel(UpgradeLevel level)
        {
            MoneyPerUpgrade = level.MoneyPerUpgrade;
            CurrentAmount = level.CurrentAmount;
            Id = level.Id;
        }

        public RuntimeUpgradeLevel()
        {
            MoneyPerUpgrade = 0;
            CurrentAmount = 0;
            Id = 0;
        }
        
        public int MoneyPerUpgrade { get; }
        public float CurrentAmount { get; }
        public int Id { get; }
    }
}