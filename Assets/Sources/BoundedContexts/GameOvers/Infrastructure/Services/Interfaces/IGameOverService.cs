using Sources.BoundedContexts.Bunkers.Domain;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.PresentationsInterfaces.Views.Constructors;

namespace Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces
{
    public interface IGameOverService : IInitialize, IDestroy
    {
    }
}