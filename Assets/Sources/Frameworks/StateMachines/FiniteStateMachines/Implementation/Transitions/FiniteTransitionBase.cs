using System;
using Sources.Frameworks.StateMachines.FiniteStateMachines.Implementation.States;

namespace Sources.Frameworks.StateMachines.FiniteStateMachines.Implementation.Transitions
{
    public class FiniteTransitionBase : FiniteTransition
    {
        private readonly Func<bool> _condition;

        public FiniteTransitionBase(
            FiniteState nextState,
            Func<bool> condition) 
            : base(nextState)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        protected override bool CanTransit() =>
            _condition.Invoke();
    }
}