using Doozy.Runtime.Reactor.Animators;
using HighlightPlus;
using UnityEngine;

namespace Sources.BoundedContexts.Bunkers.Presentation.Interfaces
{
    public interface IBunkerView
    {
        float HighlightDelta { get; }
        Vector3 Position { get; }
        UIAnimator DamageAnimator { get; }
        HighlightEffect HighlightEffect { get; }
    }
}