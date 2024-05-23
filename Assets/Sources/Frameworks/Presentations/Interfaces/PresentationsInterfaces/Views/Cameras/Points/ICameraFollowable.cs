using Sources.InfrastructureInterfaces.StateMachines.ContextStateMachines.Contexts;
using UnityEngine;

namespace Sources.PresentationsInterfaces.Views.Cameras.Points
{
    public interface ICameraFollowable : IContext
    {
        Transform Transform { get; }
    }
}