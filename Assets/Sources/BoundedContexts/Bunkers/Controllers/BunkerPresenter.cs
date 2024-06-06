using System;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;
using Sources.BoundedContexts.Enemies.PresentationInterfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Implementation;

namespace Sources.BoundedContexts.Bunkers.Controllers
{
    public class BunkerPresenter : PresenterBase
    {
        private readonly Bunker _bunker;
        private readonly IBunkerView _view;

        public BunkerPresenter(Bunker bunker, IBunkerView view)
        {
            _bunker = bunker ?? throw new ArgumentNullException(nameof(bunker));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void TakeDamage(IEnemyView enemyView)
        {
            _bunker.TakeDamage();
            enemyView.Destroy();
        }
    }
}