using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Contexts;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.States;

namespace Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Transitions
{
    public interface IContextTransition
    {
        IContextState NextState { get; }

        bool CanTransit(IContext context);
    }
}