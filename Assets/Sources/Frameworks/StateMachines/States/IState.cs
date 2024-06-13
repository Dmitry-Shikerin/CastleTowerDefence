using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;
using Sources.InfrastructureInterfaces.Services.UpdateServices.Methods;

namespace Sources.InfrastructureInterfaces.StateMachines.States
{
    public interface IState : IEnterable, IExitable, IUpdatable, ILateUpdatable, IFixedUpdatable
    {
    }
}