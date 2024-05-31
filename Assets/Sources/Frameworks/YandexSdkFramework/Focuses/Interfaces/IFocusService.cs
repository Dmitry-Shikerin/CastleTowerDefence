using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.YandexSdcFramework.Focuses.Interfaces
{
    public interface IFocusService : IInitialize, IDestroy
    {
    }
}