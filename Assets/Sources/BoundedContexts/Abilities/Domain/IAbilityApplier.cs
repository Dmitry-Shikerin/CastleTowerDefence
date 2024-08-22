using System;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.Abilities.Domain
{
    public interface IAbilityApplier : IEntity
    {
        event Action AbilityApplied;
        
        float Cooldown { get; }
        bool IsAvailable { get; set; }
        bool IsApplied { get; }
        
        void ApplyAbility();
    }
}