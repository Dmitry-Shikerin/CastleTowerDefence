using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Contexts;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Cameras.Presentation.Interfaces.Points
{
    public interface ICameraFollowable : IContext
    {
        Transform Transform { get; }
    }
}