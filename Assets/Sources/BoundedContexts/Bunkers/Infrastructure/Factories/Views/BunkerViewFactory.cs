using System;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views
{
    public class BunkerViewFactory
    {
        private readonly BunkerPresenterFactory _presenterFactory;

        public BunkerViewFactory(BunkerPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IBunkerView Create(BunkerView view)
        {
            BunkerPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);
            
            return view;
        }
    }
}