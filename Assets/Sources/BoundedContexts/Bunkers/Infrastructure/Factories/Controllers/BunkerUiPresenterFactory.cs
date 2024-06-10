using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers
{
    public class BunkerUiPresenterFactory
    {
        public BunkerUiPresenter Create(Bunker bunker, BunkerUi view)
        {
            return new BunkerUiPresenter(bunker, view);
        }
    }
}