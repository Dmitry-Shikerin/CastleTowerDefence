using System;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Contexts;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.States;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Transitions;

namespace Sources.Infrastructure.StateMachines.ContextStateMachines.Transitions
{
    public abstract class ContextTransitionBase : IContextTransition
    {
        public ContextTransitionBase(IContextState nextState)
        {
            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
        }
        
        public IContextState NextState { get; }

        public abstract bool CanTransit(IContext context);
    }
}