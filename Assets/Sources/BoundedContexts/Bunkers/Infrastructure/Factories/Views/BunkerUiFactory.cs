using System;
using Sources.BoundedContexts.Bunkers.Controllers;
using Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Bunkers.Presentation.Implementation;
using Sources.BoundedContexts.Bunkers.Presentation.Interfaces;

namespace Sources.BoundedContexts.Bunkers.Infrastructure.Factories.Views
{
    public class BunkerUiFactory
    {
        private readonly BunkerUiPresenterFactory _presenterFactory;

        public BunkerUiFactory(BunkerUiPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? 
                                throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IBunkerUi Create(BunkerUi view)
        {
            BunkerUiPresenter presenter = _presenterFactory.Create(view);
            view.Construct(presenter);

            return view;
        }
    }
}