using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Domain;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers
{
    public class BunkerPresenterFactory
    {
        public BunkerPresenter Create(Bunker bunker, IBunkerView bunkerView)
        {
            return new BunkerPresenter(bunker, bunkerView);
        }
    }
}