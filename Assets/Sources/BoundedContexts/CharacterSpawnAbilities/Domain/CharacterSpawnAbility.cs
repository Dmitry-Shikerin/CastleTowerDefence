using System;
using Sources.BoundedContexts.Abilities.Domain;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.CharacterSpawnAbilities.Domain
{
    public class CharacterSpawnAbility : IAbilityApplier, IEntity
    {
        public CharacterSpawnAbility(string id)
        {
            Id = id;
        }

        public event Action AbilityApplied;
        public float Cooldown { get; } = 0.04f;
        public bool IsAvailable { get; set; } = true;
        
        public void ApplyAbility() =>
            AbilityApplied?.Invoke();

        public string Id { get; }
        public Type Type => GetType();
    }
}