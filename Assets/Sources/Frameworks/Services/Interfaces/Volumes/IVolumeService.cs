using System;
using Sources.BoundedContexts.Volumes.Domain;
using Sources.ControllersInterfaces.ControllerLifetimes;

namespace Sources.InfrastructureInterfaces.Services.Volumes
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