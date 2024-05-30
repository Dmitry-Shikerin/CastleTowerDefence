using System;
using Sources.Domain.Models.Data;

namespace Sources.BoundedContexts.Players.Domain
{
    public class PlayerWallet
    {
        public PlayerWallet(
            int coins,
            string id)
        {
            Coins = coins;
            Id = id;
        }

        public PlayerWallet(PlayerWalletDto playerWalletDto)
        {
            Coins = playerWalletDto.Coins;
            Id = playerWalletDto.Id;
        }

        public event Action CoinsChanged;

        public string Id { get; }
        public Type Type => GetType();
        public int Coins { get; private set; }

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