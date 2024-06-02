using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.PresentationsInterfaces.Views.Constructors;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Groups
{
    [Serializable]
    public class AudioGroup : IConstruct<IAudioService>, IDestroy
    {
        [SerializeField] private List<AudioClip> _audioClips;
        [ProgressBar(0, 100, r: 0.5f, g: 0.5f, b: 0.5f, Height = 10, DrawValueLabel = false)]
        [HideLabel]
        [SerializeField] private float _currentTime;

        private IAudioService _audioService;

        public IReadOnlyList<AudioClip> AudioClips => _audioClips;
        public bool IsPlaying { get; private set; } = false;
        public AudioClip CurrentClip { get; private set; }

        public float CurrentTime => _currentTime;

        public void Construct(IAudioService soundService) =>
            _audioService = soundService ?? throw new ArgumentNullException(nameof(soundService));

        public void Destroy() =>
            _audioService = null;

        public void Play() =>
            IsPlaying = true;

        public void Stop() =>
            IsPlaying = false;

        public void SetCurrentClip(AudioClip clip) =>
            CurrentClip = clip;

        public void SetCurrentTime(float time) =>
            _currentTime = time;

        [ResponsiveButtonGroup]
        private void NextClip()
        {
            
        }

        [ResponsiveButtonGroup]
        private void PreviousClip()
        {
            
        }
    }
}