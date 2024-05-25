using System;

namespace Sources.BoundedContexts.Healths.DomainInterfaces
{
    public interface IHealth
    {
        event Action HealthChanged;
        event Action<float> DamageReceived; 
        
        float MaxHealth { get; }
        float CurrentHealth { get; }
    }
}