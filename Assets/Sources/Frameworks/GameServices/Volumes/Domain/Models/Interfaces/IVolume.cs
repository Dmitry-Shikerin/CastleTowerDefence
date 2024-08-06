using System;

namespace Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces
{
    public interface IVolume
    {
        public event Action VolumeChanged;
        
        public float VolumeValue { get; }
        public bool IsVolumeMuted { get; }
    }
}