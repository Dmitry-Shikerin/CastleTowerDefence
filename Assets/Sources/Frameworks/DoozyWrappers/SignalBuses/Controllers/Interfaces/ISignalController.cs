using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces
{
    public interface ISignalController : IInitialize, IDestroy
    {
    }
}