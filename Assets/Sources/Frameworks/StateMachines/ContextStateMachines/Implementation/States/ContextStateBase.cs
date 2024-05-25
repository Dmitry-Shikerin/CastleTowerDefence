using System.Collections.Generic;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Contexts;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.States;
using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Transitions;

namespace Sources.Infrastructure.StateMachines.ContextStateMachines.States
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