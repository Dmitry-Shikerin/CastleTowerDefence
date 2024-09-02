using System;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.BoundedContexts.GameCompleted.Infrastructure.Services.Interfaces
{
    public interface IGameCompletedService : IInitialize, IDestroy
    {
        event Action GameCompleted;
    }
}