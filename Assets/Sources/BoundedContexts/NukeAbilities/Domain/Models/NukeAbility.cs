using System;
using Sources.BoundedContexts.Abilities.Domain;

namespace Sources.BoundedContexts.NukeAbilities.Domain.Models
{
    public class NukeAbility : IAbilityApplier
    {
        public event Action AbilityApplied;
        public float Cooldown { get; private set; } = 0.5f;
        public bool IsAvailable { get; set; } = true;
        
        public void ApplyAbility() =>
            AbilityApplied?.Invoke();
    }
}