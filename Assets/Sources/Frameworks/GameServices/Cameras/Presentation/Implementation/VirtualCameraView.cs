using Cinemachine;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Cameras.Presentation.Interfaces;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
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

        public void LookAt(Transform target) =>
            _virtualCamera.LookAt = target;

        [OnInspectorInit]
        private void SetCamera()
        {
            if (_virtualCamera != null)
                return;
            
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }
}