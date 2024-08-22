using System;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models
{
    public class FlamethrowerAbility : IAbilityApplier, IEntity
    {
        private readonly Upgrade _flamethrowerAbilityUpgrade;

        public FlamethrowerAbility(Upgrade flamethrowerAbilityUpgrade, string id)
        {
            _flamethrowerAbilityUpgrade = flamethrowerAbilityUpgrade ?? 
                                          throw new ArgumentNullException(nameof(flamethrowerAbilityUpgrade));
            Id = id;
        }

        public event Action AbilityApplied;
        
        public string Id { get; }
        public Type Type => GetType();
        public float Cooldown => _flamethrowerAbilityUpgrade.CurrentAmount;
        public bool IsAvailable { get; set; } = true;
        public bool IsApplied { get; private set; }

        public void ApplyAbility()
        {
            IsApplied = true;
            AbilityApplied?.Invoke();
        }
    }
}