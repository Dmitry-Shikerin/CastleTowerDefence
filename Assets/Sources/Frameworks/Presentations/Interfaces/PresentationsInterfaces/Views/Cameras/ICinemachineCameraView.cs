using UnityEngine;

namespace Sources.PresentationsInterfaces.Views.Cameras
{
    public interface ICinemachineCameraView : IView
    {
        void Follow(Transform target);
    }
}