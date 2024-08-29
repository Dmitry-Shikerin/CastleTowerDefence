using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Cameras.Presentation.Interfaces
{
    public interface IVirtualCameraView : IView
    {
        CameraId CameraId { get; }
        
        void Follow(Transform target);
        void LookAt(Transform target);
    }
}