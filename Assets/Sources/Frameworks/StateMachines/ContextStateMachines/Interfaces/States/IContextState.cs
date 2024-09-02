using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Contexts;

namespace Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.States
{
    public interface IContextState : IEnterable, IExitable, IUpdatable
    {
        void Apply(IContext context, IContextStateChanger contextStateChanger);
    }
}