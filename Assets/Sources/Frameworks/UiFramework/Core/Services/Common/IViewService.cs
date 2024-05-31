using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.UiFramework.ServicesInterfaces;
using IDestroy = Sources.Frameworks.UiFramework.Core.Services.Common.IDestroy;

namespace Sources.Frameworks.UiFramework.Core.Services.Common
{
    public interface IViewService : IAwake, IEnable, IDisable, IDestroy
    {
    }
}