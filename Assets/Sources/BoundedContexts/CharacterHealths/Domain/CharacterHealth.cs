using System;
using Sources.BoundedContexts.Healths.DomainInterfaces;
using Sources.BoundedContexts.Upgrades.Domain.Models;
using UnityEngine;

namespace Sources.BoundedContexts.CharacterHealths.Domain
{
    public class CharacterHealth : IHealth
    {
        private readonly Upgrade _healthUpgrader;
        private float _currentHealth;

        public CharacterHealth(Upgrade healthUpgrade)
        {
            _healthUpgrader = healthUpgrade;
            CurrentHealth = MaxHealth;
        }

        public event Action HealthChanged;
        public event Action<float> DamageReceived;
        public event Action CharacterDie;

        public float MaxHealth => _healthUpgrader.CurrentAmount;
        public bool IsDied { get; private set; }

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                HealthChanged?.Invoke();
            }
        }

        public void TakeDamage(float damage)
        {
            if(CurrentHealth <= 0)
                return;
            
            CurrentHealth -= damage;
            DamageReceived?.Invoke(damage);

            if (CurrentHealth > 0)
                return;

            IsDied = true;
            CharacterDie?.Invoke();
        }

        public void TakeHeal(int heal) =>
            CurrentHealth += heal;
    }
}