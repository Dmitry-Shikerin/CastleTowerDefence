using System;
using Sources.Frameworks.Domain.Interfaces.Entities;
using UnityEngine;

namespace Sources.BoundedContexts.Bunkers.Domain
{
    public class Bunker : IEntity
    {
        private bool _isDead;

        public Bunker(int health, string id)
        {
            Health = health;
            Id = id;
        }

        public event Action OnDead;
        public event Action OnHealthChanged;

        public string Id { get; }
        public Type Type => GetType();
        public int Health { get; private set; }


        public void TakeDamage()
        {
            Health--;
            OnHealthChanged?.Invoke();

            if (Health > 0)
                return;
            
            if(_isDead)
                return;

            OnDead?.Invoke();
            _isDead = true;
        }
    }
}