using System;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;

namespace Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation
{
    public class Volume : IVolume, IEntity
    {
        private float _musicVolume = 0.1f;
        private float _soundsVolume = 0.1f;

        public Volume(string id)
        {
            Id = id;
        }

        public event Action MusicVolumeChanged;
        public event Action SoundsVolumeChanged;

        public string Id { get; }
        public Type Type => GetType();

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