using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.NukeAbilities.Presentation.Interfaces
{
    public interface IBombView : IView
    {
        Vector3 FromPosition { get; }
        Vector3 ToPosition { get; }
        Vector3 Position { get; }

        void Move();
    }
}