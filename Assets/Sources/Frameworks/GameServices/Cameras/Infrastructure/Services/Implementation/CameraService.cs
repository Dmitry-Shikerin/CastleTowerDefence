using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Cameras.Presentation.Implementation;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Cameras.Points;
using Sources.InfrastructureInterfaces.Services.Cameras;
using UnityEngine;

namespace Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Implementation
{
    public class CameraService : ICameraService
    {
        private readonly CameraView _cameraView;
        private Dictionary<Type, ICameraFollowable> _cameraTargets;
        private Dictionary<CameraId, VirtualCameraView> _virtualCameras;
        private CancellationTokenSource _token;

        public CameraService(CameraView cameraView)
        {
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _cameraTargets = new Dictionary<Type, ICameraFollowable>();
            _virtualCameras = cameraView.Cameras.ToDictionary(camera => camera.CameraId, camera => camera);
            _token = new CancellationTokenSource();
        }

        public event Action FollowableChanged;
        public event Action CameraChanged;

        public ICameraFollowable CurrentFollower { get; private set; }
        public CameraId CurrentCameraId { get; private set; }

        public async void SetOnTimeCamera(CameraId cameraId, float duration = 3f)
        {
            _token.Cancel();
            _token = new CancellationTokenSource();
            
            try
            {
                ShowCamera(cameraId);
                await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _token.Token);
                ShowCamera(CameraId.Main);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void ShowCamera(CameraId cameraId)
        {
            if (_virtualCameras.ContainsKey(cameraId) == false)
                throw new InvalidOperationException(nameof(cameraId));
            
            VirtualCameraView virtualCamera = _virtualCameras[cameraId];
            
            _cameraView.Cameras
                .Except(new List<VirtualCameraView>() {virtualCamera})
                .ToList()
                .ForEach(camera => camera.Hide());
            
            virtualCamera.Show();
            CurrentCameraId = cameraId;
            CameraChanged?.Invoke();
        }

        public void SetFollower<T>() where T : ICameraFollowable
        {
            if (_cameraTargets.ContainsKey(typeof(T)) == false)
                throw new InvalidOperationException(nameof(T));
            
            CurrentFollower = _cameraTargets[typeof(T)];
            FollowableChanged?.Invoke();
        }

        public void Add<T>(ICameraFollowable cameraFollowable) where T : ICameraFollowable
        {
            if (_cameraTargets.ContainsKey(typeof(T)))
                throw new InvalidOperationException(nameof(T));
            
            _cameraTargets[typeof(T)] = cameraFollowable;
        }

        public ICameraFollowable Get<T>() where T : ICameraFollowable
        {
            if (_cameraTargets.ContainsKey(typeof(T)) == false)
                throw new InvalidOperationException(nameof(T));

            return _cameraTargets[typeof(T)];
        }
    }
}