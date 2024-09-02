using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Contexts;

namespace Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces
{
    public interface IContextStateMachine
    {
        void Apply(IContext context);
    }
}