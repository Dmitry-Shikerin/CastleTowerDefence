using System;
using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.GameServices.Volumes.Domain.Models;

namespace Sources.Frameworks.GameServices.Volumes.Services.Interfaces
{
    public interface IVolumeService : IEnterable, IExitable
    {
        event Action MusicVolumeChanged;
        event Action MiniGunVolumeChanged;
        
        float MusicVolume { get; }
        float MiniGunVolume { get; }
        
        void Register(Volume volume);
    }
}