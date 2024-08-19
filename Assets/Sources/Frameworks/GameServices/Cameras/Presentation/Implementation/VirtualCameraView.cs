using Cinemachine;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.PresentationsInterfaces.Views.Cameras;
using UnityEngine;
namespace Sources.Frameworks.GameServices.Cameras.Presentation.Implementation
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCameraView : View, IVirtualCameraView
    {
        [Required] [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CameraId _cameraId;

        public CameraId CameraId => _cameraId;
        
        public void Follow(Transform target) =>
            _virtualCamera.Follow = target;

        [OnInspectorInit]
        private void SetCamera()
        {
            if (_virtualCamera != null)
                return;
            
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }
}