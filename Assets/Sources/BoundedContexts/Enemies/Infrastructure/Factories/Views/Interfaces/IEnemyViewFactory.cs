using Sources.BoundedContexts.Enemies.Domain;
using Sources.BoundedContexts.Enemies.Domain.Models;
using Sources.BoundedContexts.Enemies.Presentation;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;

namespace Sources.BoundedContexts.Enemies.Infrastructure.Factories.Views.Interfaces
{
    public interface IEnemyViewFactory
    {
        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter, PlayerWallet playerWallet);
        public IEnemyView Create(Enemy enemy, KillEnemyCounter killEnemyCounter, PlayerWallet playerWallet, EnemyView view);
    }
}