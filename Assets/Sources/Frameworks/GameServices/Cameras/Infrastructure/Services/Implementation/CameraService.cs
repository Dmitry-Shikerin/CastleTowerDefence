using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Interfaces;
using Sources.Frameworks.GameServices.Cameras.Presentation.Implementation;
using Sources.Frameworks.GameServices.Cameras.Presentation.Interfaces.Points;
using UnityEngine;
using UnityEngine.Playables;

namespace Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Implementation
{
    public class CameraService : ICameraService
    {
        private readonly CameraView _cameraView;
        private Dictionary<Type, ICameraFollowable> _cameraTargets;
        private Dictionary<CameraId, VirtualCameraView> _virtualCameras;
        private Dictionary<DirectorId, PlayableDirectorView> _directors;
        private CancellationTokenSource _token;
        private bool _isDirectorPlaying;

        public CameraService(CameraView cameraView)
        {
            _cameraView = cameraView ?? throw new ArgumentNullException(nameof(cameraView));
            _cameraTargets = new Dictionary<Type, ICameraFollowable>();
            _virtualCameras = cameraView.Cameras.ToDictionary(camera => camera.CameraId, camera => camera);
            _directors = cameraView.Directors.ToDictionary(director => director.DirectorId, director => director);
            _token = new CancellationTokenSource();
        }

        public event Action FollowableChanged;
        public event Action CameraChanged;

        public ICameraFollowable CurrentFollower { get; private set; }
        public CameraId CurrentCameraId { get; private set; }

        public async void PlayDirector(DirectorId directorId)
        {
            if (_directors.ContainsKey(directorId) == false)
                throw new KeyNotFoundException(directorId.ToString());
            
            _token.Cancel();
            _token = new CancellationTokenSource();
            PlayableDirectorView director = _directors[directorId];
            
            try
            {
                HideCameras();
                director.Play();
                Debug.Log($"Start play");
                _isDirectorPlaying = true;
                await UniTask.WaitWhile(() => 
                    director.PlayState == PlayState.Playing, 
                    cancellationToken: _token.Token);
                Debug.Log($"Stop play");
                _isDirectorPlaying = false;
            }
            catch (OperationCanceledException)
            {
            }
        }

        public async void SetOnTimeCamera(CameraId cameraId, float duration = 3f)
        {
            if (_isDirectorPlaying)
                return;
            
            if (_virtualCameras.ContainsKey(cameraId) == false)
                throw new KeyNotFoundException(cameraId.ToString());
            
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

        private void HideCameras()
        {
            _cameraView.Cameras
                .ToList()
                .ForEach(camera => camera.Hide());
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

        public void Destroy() =>
            _token.Cancel();
    }
}