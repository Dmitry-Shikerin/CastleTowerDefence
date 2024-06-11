using System;
using Sources.BoundedContexts.EnemySpawners.Controllers;
using Sources.BoundedContexts.EnemySpawners.Domain;
using Sources.BoundedContexts.EnemySpawners.Domain.Models;
using Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.EnemySpawners.Presentation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Implementation;
using Sources.BoundedContexts.EnemySpawners.Presentation.Interfaces;
using Sources.BoundedContexts.KillEnemyCounters.Domain;
using Sources.BoundedContexts.PlayerWallets.Domain.Models;

namespace Sources.BoundedContexts.EnemySpawners.Infrastructure.Factories.Views
{
    public class EnemySpawnerViewFactory
    {
        private readonly EnemySpawnerPresenterFactory _presenterFactory;

        public EnemySpawnerViewFactory(EnemySpawnerPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }
        
        public IEnemySpawnerView Create(
            EnemySpawner enemySpawner, 
            KillEnemyCounter killEnemyCounter,
            PlayerWallet playerWallet,
            EnemySpawnerView view)
        {
            EnemySpawnerPresenter presenter = _presenterFactory.Create(
                enemySpawner, killEnemyCounter, playerWallet, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}