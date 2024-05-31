using System;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;

namespace Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation
{
    public class Volume : IVolume
    {
        public event Action MusicVolumeChanged;
        public event Action MiniGunVolumeChanged;
        
        public float SoundsVolume { get; set; }
        public float MusicVolume { get; set; }
        public bool IsSoundsMuted { get; }
        public bool IsMusicMuted { get; }
    }
}