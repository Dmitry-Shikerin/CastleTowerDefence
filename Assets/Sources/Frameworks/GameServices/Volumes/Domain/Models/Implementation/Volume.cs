using System;
using Sources.Frameworks.Domain.Interfaces.Entities;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;

namespace Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation
{
    public class Volume : IVolume, IEntity
    {
        private float _volumeValue = 0.2f;
        private bool _isVolumeMuted;

        public event Action VolumeChanged;
        public event Action VolumeMuted;

        public string Id { get; set; }
        public Type Type => GetType();

        public float VolumeValue
        {
            get => _volumeValue;
            set
            {
                _volumeValue = value;
                VolumeChanged?.Invoke();
            }
        }

        public bool IsVolumeMuted
        {
            get => _isVolumeMuted;
            set
            {
                _isVolumeMuted = value;
                VolumeMuted?.Invoke();
            }
        }
    }
}