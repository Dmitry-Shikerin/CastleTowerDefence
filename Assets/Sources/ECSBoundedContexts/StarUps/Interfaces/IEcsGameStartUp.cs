using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.ECSBoundedContexts.StarUps.Interfaces
{
    public interface IEcsGameStartUp : IInitialize, IUpdatable, IDestroy
    {
    }
}