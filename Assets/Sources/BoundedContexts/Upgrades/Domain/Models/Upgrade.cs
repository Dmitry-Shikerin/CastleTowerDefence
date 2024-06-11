using System;
using System.Collections.Generic;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Configs;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.Upgrades.Domain.Models
{
    public class Upgrade : IEntity
    {
        public Upgrade(UpgradeConfig config)
        {
            Levels = config.Levels;
            Id = config.Id;
        }

        public event Action LevelChanged;

        public string Id { get; }
        public Type Type => GetType();
        public IReadOnlyList<UpgradeLevel> Levels  { get; private set; }
        public float CurrentAmount => Levels[CurrentLevel].CurrentAmount;
        public int CurrentLevel { get; private set; }
        public int MaxLevel => Levels.Count;
        
        public void ApplyUpgrade(PlayerWallet wallet)
        {
            if (CurrentLevel >= MaxLevel)
                return;
            
            if(wallet.TryRemoveCoins(Levels[CurrentLevel].MoneyPerUpgrade) == false)
                return;
            
            CurrentLevel++;
            LevelChanged?.Invoke();
        }
    }
}