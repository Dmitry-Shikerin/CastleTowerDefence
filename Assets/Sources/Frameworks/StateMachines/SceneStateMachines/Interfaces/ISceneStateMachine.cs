using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.GameServices.UpdateServices.Interfaces.Methods;
using Sources.InfrastructureInterfaces.Services.UpdateServices.Methods;
using Sources.InfrastructureInterfaces.StateMachines.States;

namespace Sources.InfrastructureInterfaces.StateMachines.SceneStateMachines
{
    public interface ISceneStateMachine : IUpdatable, ILateUpdatable, IFixedUpdatable, IExitable
    {
        void ChangeState(IState state, object payload = null);
    }
}