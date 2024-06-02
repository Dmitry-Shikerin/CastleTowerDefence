using System;

namespace Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces
{
    public interface IVolume
    {
        public event Action MusicVolumeChanged;
        public event Action SoundsVolumeChanged;
        
        public float SoundsVolume { get; }
        public float MusicVolume { get; }
        public bool IsSoundsMuted { get; }
        public bool IsMusicMuted { get; }
    }
}