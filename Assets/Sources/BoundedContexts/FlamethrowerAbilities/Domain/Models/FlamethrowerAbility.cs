using System;
using Sources.BoundedContexts.Abilities.Domain;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Domain.Models
{
    public class FlamethrowerAbility : IAbilityApplier
    {
        public event Action AbilityApplied;
        public float Cooldown { get; } = 0.5f;
        public bool IsAvailable { get; set; } = true;
        public void ApplyAbility() => 
            AbilityApplied?.Invoke();
    }
}