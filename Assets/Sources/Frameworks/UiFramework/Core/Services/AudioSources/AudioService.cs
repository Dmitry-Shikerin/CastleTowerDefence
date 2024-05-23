using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Frameworks.UiFramework.Presentation.AudioSources;
using Sources.Frameworks.UiFramework.Presentation.AudioSources.Types;
using Sources.Frameworks.UiFramework.Presentation.Forms;
using Sources.Frameworks.UiFramework.Presentation.Views;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.AudioSources;
using Sources.Frameworks.UiFramework.ServicesInterfaces.AudioSources;
using Sources.InfrastructureInterfaces.Services.Volumes;

namespace Sources.Frameworks.UiFramework.Services.AudioSources
{
    public class AudioService : IAudioService
    {
        private readonly IVolumeService _volumeService;
        private readonly Dictionary<AudioSourceId, IUiAudioSource> _audioSources;
        
        public AudioService(
            UiCollector uiCollector,
            IVolumeService volumeService)
        {
            _volumeService = volumeService ?? throw new ArgumentNullException(nameof(volumeService));
            _audioSources = uiCollector.UiAudioSources.ToDictionary(
                uiAudioSource => uiAudioSource.AudioSourceId, uiAudioSource => uiAudioSource);
        }

        public void Enter(object payload = null)
        {
            OnVolumeChanged();
            _volumeService.MusicVolumeChanged += OnVolumeChanged;
        }

        public void Exit() =>
            _volumeService.MusicVolumeChanged -= OnVolumeChanged;

        private void OnVolumeChanged()
        {
            foreach (IUiAudioSource audioSource in _audioSources.Values)
                audioSource.SetVolume(_volumeService.MusicVolume);
        }

        public void Play(AudioSourceId id)
        {
            if(_audioSources.ContainsKey(id) == false)
                throw new KeyNotFoundException(id.ToString());
            
            _audioSources[id].Play();
        }
    }
}