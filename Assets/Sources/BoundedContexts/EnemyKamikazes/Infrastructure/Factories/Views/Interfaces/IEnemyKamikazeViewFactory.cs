using Sources.BoundedContexts.EnemyKamikazes.Domain;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Implementation;
using Sources.BoundedContexts.EnemyKamikazes.Presentations.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;

namespace Sources.BoundedContexts.EnemyKamikazes.Infrastructure.Factories.Views.Interfaces
{
    public interface IEnemyKamikazeViewFactory
    {
        IEnemyKamikazeView Create(EnemyKamikaze enemy, KillEnemyCounter killEnemyCounter, PlayerWallet playerWallet);
        IEnemyKamikazeView Create(
            EnemyKamikaze enemy, KillEnemyCounter killEnemyCounter, PlayerWallet playerWallet, EnemyKamikazeView view);
    }
}