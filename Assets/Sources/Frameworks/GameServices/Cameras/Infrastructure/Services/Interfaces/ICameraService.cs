using System;
using Sources.Frameworks.GameServices.Cameras.Domain;
using Sources.Frameworks.GameServices.Cameras.Presentation.Interfaces.Points;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;

namespace Sources.Frameworks.GameServices.Cameras.Infrastructure.Services.Interfaces
{
    public interface ICameraService : IDestroy
    {
        event Action FollowableChanged;
        event Action CameraChanged;
        
        ICameraFollowable CurrentFollower { get; }
        CameraId CurrentCameraId { get; }

        void PlayDirector(DirectorId directorId);
        void SetOnTimeCamera(CameraId cameraId, float duration = 3f);
        void SetFollower<T>() where T : ICameraFollowable;
        void Add<T>(ICameraFollowable cameraFollowable) where T : ICameraFollowable;
        ICameraFollowable Get<T>() where T : ICameraFollowable;
    }
}