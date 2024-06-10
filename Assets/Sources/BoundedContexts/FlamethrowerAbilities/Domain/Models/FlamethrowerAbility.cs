using System;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Upgrades.Domain.Models;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models
{
    public class FlamethrowerAbility : IAbilityApplier
    {
        private readonly Upgrade _flamethrowerAbilityUpgrade;

        public FlamethrowerAbility(Upgrade flamethrowerAbilityUpgrade)
        {
            _flamethrowerAbilityUpgrade = flamethrowerAbilityUpgrade ?? 
                                          throw new ArgumentNullException(nameof(flamethrowerAbilityUpgrade));
        }

        public event Action AbilityApplied;
        public float Cooldown => _flamethrowerAbilityUpgrade.CurrentAmount;
        public bool IsAvailable { get; set; } = true;
        public void ApplyAbility() => 
            AbilityApplied?.Invoke();
    }
}