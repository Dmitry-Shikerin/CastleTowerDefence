using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.UiFramework.ServicesInterfaces;

namespace Sources.Frameworks.UiFramework.Core.Services.Common
{
    public interface IViewService : IAwake, IEnable, IDisable, IDestroy
    {
    }
}