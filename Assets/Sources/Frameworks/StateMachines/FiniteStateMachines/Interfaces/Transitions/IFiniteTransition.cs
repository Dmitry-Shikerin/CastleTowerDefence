using Sources.Frameworks.StateMachines.FiniteStateMachines.Implementation.States;

namespace Sources.Frameworks.StateMachines.FiniteStateMachines.Interfaces.Transitions
{
    public interface IFiniteTransition
    {
        bool CanMoveNextState(out FiniteState nextState);
    }
}