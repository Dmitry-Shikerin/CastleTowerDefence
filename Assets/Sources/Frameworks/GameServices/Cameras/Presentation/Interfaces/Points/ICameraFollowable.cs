using Sources.Frameworks.StateMachines.ContextStateMachines.Interfaces.Contexts;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Cameras.Presentation.Interfaces.Points
{
    public interface ICameraFollowable : IContext
    {
        Transform Transform { get; }
    }
}