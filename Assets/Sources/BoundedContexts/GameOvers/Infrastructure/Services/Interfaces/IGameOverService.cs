using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.BoundedContexts.GameOvers.Infrastructure.Services.Interfaces
{
    public interface IGameOverService : IInitialize, IDestroy
    {
    }
}