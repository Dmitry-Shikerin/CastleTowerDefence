namespace Sources.BoundedContexts.PlayerWallet.Domain
{
    public class PlayerWallet
    {
        private int _coins;

        public void AddCoins(int coins)
        {
            _coins += coins;
        }
    }
}