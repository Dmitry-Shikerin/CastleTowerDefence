using System;
using UnityEngine;

namespace Sources.BoundedContexts.Bunkers.Domain
{
    public class Bunker
    {
        private bool _isDead;

        public Bunker(int health)
        {
            Health = health;
        }

        public event Action OnDead;
        public event Action OnHealthChanged;

        public int Health { get; private set; }

        public void TakeDamage()
        {
            Debug.Log("Bunker dealDamage");
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