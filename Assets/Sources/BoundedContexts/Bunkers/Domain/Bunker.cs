using System;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.Bunkers.Domain
{
    public class Bunker : IEntity
    {
        private bool _isDead;

        public event Action Death;
        public event Action HealthChanged;

        public string Id { get; set; }
        public Type Type => GetType();
        public int Health { get; set; }

        public void TakeDamage()
        {
            Health--;
            HealthChanged?.Invoke();

            if (Health > 0)
                return;
            
            if(_isDead)
                return;

            Death?.Invoke();
            _isDead = true;
        }

        public void ApplyBoost()
        {
            Health ++;
            HealthChanged?.Invoke();
        }
    }
}