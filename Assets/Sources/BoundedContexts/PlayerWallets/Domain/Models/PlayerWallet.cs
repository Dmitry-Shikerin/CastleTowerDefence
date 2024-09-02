using System;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.PlayerWallets.Domain.Models
{
    public class PlayerWallet : IEntity
    {
        public event Action CoinsChanged;

        public string Id { get; set; }
        public Type Type => GetType();
        public int Coins { get; set; }

        public void AddCoins(int amount)
        {
            Coins += amount;
            CoinsChanged?.Invoke();
        }

        public bool TryRemoveCoins(int amount)
        {
            if (Coins < amount)
                return false;
            
            Coins -= amount;
            CoinsChanged?.Invoke();
            return true;
        }
    }
}