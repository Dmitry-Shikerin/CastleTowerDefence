using System;

namespace Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.AdverticingServices
{
    public interface IVideoAdService
    {
        bool IsAvailable { get; }
        
        void ShowVideo(Action onCloseCallback);
    }
}