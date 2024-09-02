using System.Collections.Generic;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Contexts;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.States;
using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Transitions;

namespace Sources.Frameworks.StateMachines.ContextStateMachines.Implementation.States
{
    public abstract class ContextStateBase : IContextState
    {
        private readonly List<IContextTransition> _transitions = new List<IContextTransition>();

        public virtual void Enter(object payload = null)
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update(float deltaTime)
        {
        }

        public void AddTransition(IContextTransition transition) => 
            _transitions.Add(transition);

        public void RemoveTransition(IContextTransition transition) => 
            _transitions.Remove(transition);

        public void Apply(IContext context, IContextStateChanger contextStateChanger)
        {
            foreach (IContextTransition transition in _transitions)
            {
                if(transition.CanTransit(context) == false)
                    continue;
                
                contextStateChanger.ChangeState(transition.NextState);
                transition.NextState.Apply(context, contextStateChanger);
            }
        }
    }
}