using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Controllers.Interfaces.Collectors
{
    public interface ISignalControllersCollector : IInitialize, IDestroy
    {
    }
}