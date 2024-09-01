using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;
using Sources.BoundedContexts.Upgrades.Domain.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.Upgrades.Domain.Models
{
    public class Upgrade : IEntity
    {
        public event Action LevelChanged;

        public string Id { get; set; }
        public Type Type => GetType();
        public List<RuntimeUpgradeLevel> Levels  { get; set; }
        public RuntimeUpgradeConfig Config { get; set; }
        public int CurrentLevel { get; set; }
        [JsonIgnore]
        public float CurrentAmount => Levels[CurrentLevel].CurrentAmount;
        [JsonIgnore]
        public int MaxLevel => Levels.Count - 1;

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