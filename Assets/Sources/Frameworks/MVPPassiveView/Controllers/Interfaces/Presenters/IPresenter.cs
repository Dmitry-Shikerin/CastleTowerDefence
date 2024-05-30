using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;

namespace Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.Presenters
{
    public interface IPresenter : IInitialize, IEnable, IDisable, IDestroy
    {
    }
}