using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Contexts;

namespace Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines
{
    public interface IContextStateMachine
    {
        void Apply(IContext context);
    }
}