using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.States;

namespace Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces
{
    public interface IContextStateChanger
    {
        void ChangeState(IContextState state);
    }
}