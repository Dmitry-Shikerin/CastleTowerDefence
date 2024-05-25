using Sources.Infrastructure.StateMachines.FiniteStateMachines.States;

namespace Sources.InfrastructureInterfaces.StateMachines.FiniteStateMachines.Transitions
{
    public interface IFiniteTransition
    {
        bool CanMoveNextState(out FiniteState nextState);
    }
}