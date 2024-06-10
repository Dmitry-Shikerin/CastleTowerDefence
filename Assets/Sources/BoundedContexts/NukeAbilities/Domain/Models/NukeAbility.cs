using System;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Upgrades.Domain.Models;

namespace Sources.BoundedContexts.NukeAbilities.Domain.Models
{
    public class NukeAbility : IAbilityApplier
    {
        private readonly Upgrade _nukeAbilityUpgrade;

        public NukeAbility(Upgrade nukeAbilityUpgrade)
        {
            _nukeAbilityUpgrade = nukeAbilityUpgrade 
                                  ?? throw new ArgumentNullException(nameof(nukeAbilityUpgrade));
        }

        public event Action AbilityApplied;
        
        public float Cooldown => _nukeAbilityUpgrade.CurrentAmount;
        public bool IsAvailable { get; set; } = true;
        
        public void ApplyAbility() =>
            AbilityApplied?.Invoke();
    }
}