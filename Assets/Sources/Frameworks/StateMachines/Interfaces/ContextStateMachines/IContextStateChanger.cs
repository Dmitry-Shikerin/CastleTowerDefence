using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.States;

namespace Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines
{
    public interface IContextStateChanger
    {
        void ChangeState(IContextState state);
    }
}