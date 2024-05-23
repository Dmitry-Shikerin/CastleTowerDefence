using System;
using Sources.Infrastructure.StateMachines.FiniteStateMachines.States;

namespace Sources.Infrastructure.StateMachines.FiniteStateMachines.Transitions
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