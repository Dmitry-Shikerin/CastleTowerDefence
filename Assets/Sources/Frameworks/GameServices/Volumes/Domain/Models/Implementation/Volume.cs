using System;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;

namespace Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation
{
    public class Volume : IVolume
    {
        //TODO не забыть убрать
        private float _musicVolume = 0.1f;
        private float _soundsVolume = 0.1f;
        public event Action MusicVolumeChanged;
        public event Action SoundsVolumeChanged;

        public float SoundsVolume
        {
            get => _soundsVolume;
            set
            {
                _soundsVolume = value;
                SoundsVolumeChanged?.Invoke();
            }
        }

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                MusicVolumeChanged?.Invoke();
            }
        }

        public bool IsSoundsMuted { get; }
        public bool IsMusicMuted { get; }
    }
}