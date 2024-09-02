using System;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Contexts;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.States;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Transitions;

namespace Sources.Frameworks.StateMachines.ContextStateMachines.Implementation.Transitions
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