using System;
using Sources.Frameworks.StateMachines.FiniteStateMachines.Implementation.States;
using Sources.Frameworks.StateMachines.FiniteStateMachines.Interfaces.Transitions;

namespace Sources.Frameworks.StateMachines.FiniteStateMachines.Implementation.Transitions
{
    public abstract class FiniteTransition : IFiniteTransition
    {
        private readonly FiniteState _nextState;

        protected FiniteTransition(FiniteState nextState)
        {
            _nextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
        }

        public bool CanMoveNextState(out FiniteState state)
        {
            state = _nextState;

            return CanTransit();
        }

        protected abstract bool CanTransit();
    }
}