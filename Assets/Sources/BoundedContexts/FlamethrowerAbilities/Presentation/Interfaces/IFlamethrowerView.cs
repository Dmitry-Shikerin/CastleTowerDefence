using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.BoundedContexts.FlamethrowerAbilities.Presentation.Interfaces
{
    public interface IFlamethrowerView : IView
    {
        Vector3 FromPosition { get; }
        Vector3 ToPosition { get; }
        Vector3 Position { get; }

        void Move(Vector3 targetPosition);
    }
}