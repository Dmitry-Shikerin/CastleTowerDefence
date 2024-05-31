using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;
using Sources.Frameworks.Services.ObjectPools.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Services.Interfaces;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;

namespace Sources.Frameworks.UiFramework.AudioSources.Services.Implementation
{
    public class AudioService : IAudioService
    {
        private readonly Dictionary<AudioSourceId, IUiAudioSource> _audioSources;
        private readonly ObjectPool<UiAudioSource> _audioSourcePool = new ObjectPool<UiAudioSource>();
        
        private IVolume _volume;

        public AudioService(UiCollector uiCollector)
        {
            _audioSources = uiCollector.UiAudioSources.ToDictionary(
                uiAudioSource => uiAudioSource.AudioSourceId, uiAudioSource => uiAudioSource);
        }

        public void Initialize()
        {
            if (_volume == null)
                throw new NullReferenceException(nameof(_volume));
            
            OnVolumeChanged();
            _volume.MusicVolumeChanged += OnVolumeChanged;
        }

        public void Destroy() =>
            _volume.MusicVolumeChanged -= OnVolumeChanged;

        private void OnVolumeChanged()
        {
            foreach (IUiAudioSource audioSource in _audioSources.Values)
                audioSource.SetVolume(_volume.MusicVolume);
        }

        public void Play(AudioSourceId id)
        {
            if(_audioSources.ContainsKey(id) == false)
                throw new KeyNotFoundException(id.ToString());
            
            _audioSources[id].Play();
        }

        public void Construct(IVolume volume) =>
            _volume = volume ?? throw new ArgumentNullException(nameof(volume));
    }
}