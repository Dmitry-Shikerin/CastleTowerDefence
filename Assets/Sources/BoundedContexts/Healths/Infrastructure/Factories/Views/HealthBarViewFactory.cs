using System;
using Sources.BoundedContexts.Healths.DomainInterfaces;
using Sources.BoundedContexts.Healths.Infrastructure.Factories.Controllers;
using Sources.BoundedContexts.Healths.Presentation.Implementation;
using Sources.BoundedContexts.Healths.Presentation.Interfaces;

namespace Sources.BoundedContexts.Healths.Infrastructure.Factories.Views
{
    public class HealthBarViewFactory
    {
        private readonly HealthBarPresenterFactory _presenterFactory;

        public HealthBarViewFactory(HealthBarPresenterFactory presenterFactory)
        {
            _presenterFactory = presenterFactory ?? throw new ArgumentNullException(nameof(presenterFactory));
        }

        public IHealthBarView Create(IHealth health, HealthBarView view)
        {
            var presenter = _presenterFactory.Create(health, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}