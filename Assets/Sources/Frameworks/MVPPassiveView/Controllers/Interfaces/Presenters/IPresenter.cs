using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;

namespace Sources.ControllersInterfaces
{
    public interface IPresenter : IEnable, IDisable
    {
    }
}