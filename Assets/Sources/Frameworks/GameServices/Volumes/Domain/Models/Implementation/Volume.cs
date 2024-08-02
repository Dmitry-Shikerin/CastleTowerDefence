using System;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;

namespace Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation
{
    //todo разбить на две модели дубляэ в вольюме, а и вьшки фабрики объединить
    public class Volume : IVolume, IEntity
    {
        private float _musicVolume = 0.2f;
        private float _soundsVolume = 0.2f;
        private bool _isMusicMuted;
        private bool _isSoundMuted;

        public Volume(string id)
        {
            Id = id;
        }

        public event Action MusicVolumeChanged;
        public event Action SoundsVolumeChanged;
        public event Action MusicMuted;
        public event Action SoundMuted;

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

        public bool IsSoundsMuted
        {
            get => _isSoundMuted;
            set
            {
                _isSoundMuted = value;
                SoundMuted?.Invoke();
            }
        }

        public bool IsMusicMuted
        {
            get => _isMusicMuted;
            set
            {
                _isMusicMuted = value;
                MusicMuted?.Invoke();
            }
        }
    }
}