using System;
using System.Collections.Generic;
using Sources.InfrastructureInterfaces.Services.Cameras;
using Sources.PresentationsInterfaces.Views.Cameras.Points;

namespace Sources.Infrastructure.Services.Cameras
{
    public class CameraService : ICameraService
    {
        private Dictionary<Type, ICameraFollowable> _cameraTargets = new Dictionary<Type, ICameraFollowable>();

        public event Action FollowableChanged;
        
        public ICameraFollowable CurrentFollower { get; private set; }

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