using Sources.BoundedContexts.Healths.Controllers;
using Sources.BoundedContexts.Healths.DomainInterfaces;
using Sources.BoundedContexts.Healths.Presentation.Implementation;
using Sources.BoundedContexts.Healths.Presentation.Interfaces;

namespace Sources.BoundedContexts.Healths.Infrastructure.Factories.Views
{
    public class HealthBarViewFactory
    {
        public IHealthBarView Create(IHealth health, HealthBarView view)
        {
            HealthBarPresenter presenter = new HealthBarPresenter(health, view);
            view.Construct(presenter);
            
            return view;
        }
    }
}