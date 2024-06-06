using System;
using Sources.BoundedContexts.Abilities.Domain;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Domain
{
    public class CharacterSpawnAbility : IAbilityApplier
    {
        public event Action AbilityApplied;
        public float Cooldown { get; }
        public bool IsAvailable { get; set; }
        
        public void ApplyAbility() =>
            AbilityApplied?.Invoke();
    }
}