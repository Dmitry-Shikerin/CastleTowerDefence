using System;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Domain
{
    public class CharacterSpawnAbility : IAbilityApplier, IEntity
    {
        public event Action AbilityApplied;
        
        public float Cooldown { get; } = 0.04f;
        public bool IsAvailable { get; set; } = true;
        public bool IsApplied { get; private set; }
        
        public void ApplyAbility()
        {
            IsApplied = true;
            AbilityApplied?.Invoke();
        }

        public string Id { get; set; }
        public Type Type => GetType();
    }
}