using Doozy.Runtime.Reactor.Animators;
using UnityEngine;

namespace Sources.BoundedContexts.Bunkers.Presentation.Interfaces
{
    public interface IBunkerView
    {
        Vector3 Position { get; }
        UIAnimator DamageAnimator { get; }
    }
}