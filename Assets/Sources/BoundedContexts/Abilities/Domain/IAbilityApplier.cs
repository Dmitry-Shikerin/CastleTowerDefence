using System;

namespace Sources.BoundedContexts.Abilities.Domain
{
    public interface IAbilityApplier
    {
        event Action AbilityApplied;
        
        float Cooldown { get; }
        bool IsAvailable { get; set; }
        
        void ApplyAbility();
    }
}