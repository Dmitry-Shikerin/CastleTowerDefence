using System;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.BoundedContexts.GameCompleteds.Infrastructure.Services.Interfaces
{
    public interface IGameCompletedService : IInitialize, IDestroy
    {
        event Action GameCompleted;
    }
}