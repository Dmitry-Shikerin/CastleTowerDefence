using System;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Contexts;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.States;

namespace Sources.Frameworks.StateMachines.ContextStateMachines.Implementation.Transitions
{
    public class FuncContextTransition : ContextTransitionBase
    {
        private readonly Func<IContext, bool> _condition;

        public FuncContextTransition(IContextState nextState, Func<IContext , bool> condition) 
            : base(nextState)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public override bool CanTransit(IContext context) =>
            _condition.Invoke(context);
    }
}