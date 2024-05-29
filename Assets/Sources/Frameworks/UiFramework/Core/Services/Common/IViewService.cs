using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.UiFramework.ServicesInterfaces;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;

namespace Sources.Frameworks.UiFramework.Core.Services.Common
{
    public interface IViewService : IAwake, IEnable, IDisable, IDestroy
    {
    }
}