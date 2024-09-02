using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Contexts;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.States;

namespace Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Transitions
{
    public interface IContextTransition
    {
        IContextState NextState { get; }

        bool CanTransit(IContext context);
    }
}