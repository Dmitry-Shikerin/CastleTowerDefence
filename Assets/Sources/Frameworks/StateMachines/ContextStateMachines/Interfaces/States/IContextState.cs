using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.InfrastructureInterfaces.Services.UpdateServices.Methods;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Contexts;

namespace Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.States
{
    public interface IContextState : IEnterable, IExitable, IUpdatable
    {
        void Apply(IContext context, IContextStateChanger contextStateChanger);
    }
}