using System;
using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.InfrastructureInterfaces.Services.StatesLifetimes;
using Sources.PresentationsInterfaces.Views.Cameras.Points;
using UnityEngine;

namespace Sources.InfrastructureInterfaces.Services.Cameras
{
    public interface ICameraService
    {
        event Action FollowableChanged;
        
        ICameraFollowable CurrentFollower { get; }
        
        void SetFollower<T>() where T : ICameraFollowable;
        void Add<T>(ICameraFollowable cameraFollowable) where T : ICameraFollowable;
        ICameraFollowable Get<T>() where T : ICameraFollowable;
    }
}