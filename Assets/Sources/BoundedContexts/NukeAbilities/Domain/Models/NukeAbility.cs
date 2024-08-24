using System;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.NukeAbilities.Domain.Models
{
    public class NukeAbility : IAbilityApplier, IEntity
    {
        private readonly Upgrade _nukeAbilityUpgrade;

        public NukeAbility(Upgrade nukeAbilityUpgrade, string id)
        {
            _nukeAbilityUpgrade = nukeAbilityUpgrade 
                                  ?? throw new ArgumentNullException(nameof(nukeAbilityUpgrade));
            Id = id;
        }

        public event Action AbilityApplied;

        public string Id { get; }
        public Type Type => GetType();
        public float Cooldown => _nukeAbilityUpgrade.CurrentAmount;
        public bool IsAvailable { get; set; } = true;
        public bool IsApplied { get; private set; }
        public int Damage { get; } = 1000;


        public void ApplyAbility()
        {
            IsApplied = true;
            AbilityApplied?.Invoke();
        }
    }
}