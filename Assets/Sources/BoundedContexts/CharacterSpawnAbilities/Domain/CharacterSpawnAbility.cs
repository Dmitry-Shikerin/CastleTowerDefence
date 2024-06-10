using System;
using Sources.BoundedContexts.Abilities.Domain;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Domain
{
    public class CharacterSpawnAbility : IAbilityApplier
    {
        public event Action AbilityApplied;
        public float Cooldown { get; } = 0.04f;
        public bool IsAvailable { get; set; } = true;
        
        public void ApplyAbility() =>
            AbilityApplied?.Invoke();
    }
}