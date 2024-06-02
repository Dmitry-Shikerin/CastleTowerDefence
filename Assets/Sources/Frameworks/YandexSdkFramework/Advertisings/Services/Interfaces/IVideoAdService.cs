using System;

namespace Sources.Frameworks.YandexSdkFramework.Advertisings.Services.Interfaces
{
    public interface IVideoAdService
    {
        bool IsAvailable { get; }
        
        void ShowVideo(Action onCloseCallback);
    }
}