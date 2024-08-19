using System;
using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Cameras.Points;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Services.Cameras
{
    public interface ICameraService
    {
        event Action FollowableChanged;
        event Action CameraChanged;
        
        ICameraFollowable CurrentFollower { get; }
        CameraId CurrentCameraId { get; }

        void SetOnTimeCamera(CameraId cameraId, float duration = 3f);
        void SetFollower<T>() where T : ICameraFollowable;
        void Add<T>(ICameraFollowable cameraFollowable) where T : ICameraFollowable;
        ICameraFollowable Get<T>() where T : ICameraFollowable;
    }
}