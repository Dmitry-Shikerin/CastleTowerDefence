using Sources.BoundedContexts.Healths.Controllers;
using Sources.BoundedContexts.Healths.DomainInterfaces;
using Sources.BoundedContexts.Healths.Presentation.Interfaces;

namespace Sources.BoundedContexts.Healths.Infrastructure.Factories.Controllers
{
    public class HealthBarPresenterFactory
    {
        public HealthBarPresenter Create(IHealth health, IHealthBarView view)
        {
            return new HealthBarPresenter(health, view);
        }
    }
}