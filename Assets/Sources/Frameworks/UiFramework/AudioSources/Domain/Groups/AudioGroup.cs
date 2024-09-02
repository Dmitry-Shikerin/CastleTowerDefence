using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Constructors;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.Utils.Extensions;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Groups
{
    [Serializable]
    public class AudioGroup : IConstruct<IAudioService>, IDestroy
    {
        [SerializeField] private List<AudioClip> _audioClips;
        [BoxGroup("CurrentClip")] [HideLabel]
        [SerializeField] private AudioClip _currentClip;
        [BoxGroup("CurrentClip", showLabel: false)]
        [ProgressBar(0, 100, r: 255f, g: 215f, b: 0.5f, Height = 10, DrawValueLabel = false)]
        [HideLabel]
        [SerializeField] private float _currentTime;

        private IAudioService _audioService;

        public IReadOnlyList<AudioClip> AudioClips => _audioClips;
        public bool IsPlaying { get; private set; } = false;
        public AudioClip CurrentClip => _currentClip;

        public float CurrentTime => _currentTime;

        public void Construct(IAudioService leaderBoardElementViews) =>
            _audioService = leaderBoardElementViews ?? throw new ArgumentNullException(nameof(leaderBoardElementViews));

        public void Destroy()
        {
            _audioService = null;
            _currentClip = null;
            _currentTime = 0;
        }

        public void Play() =>
            IsPlaying = true;

        public void Stop() =>
            IsPlaying = false;

        public void SetCurrentClip(AudioClip clip) =>
            _currentClip = clip;

        public void SetCurrentTime(float time) =>
            _currentTime = time.FloatToPercent(_currentClip.length);

        [ButtonGroup("CurrentClip/Buttons")] 
        [Button(SdfIconType.ArrowLeft)] 
        [HideLabel]
        private void PreviousClip()
        {
            
        }

        [Button(SdfIconType.Pause)]
        [ButtonGroup("CurrentClip/Buttons")] 
        [HideLabel] 
        private void PauseClip()
        {
            
        }

        [ButtonGroup("CurrentClip/Buttons")] 
        [Button(SdfIconType.ArrowRight)] 
        [HideLabel]
        private void NextClip()
        {
            
        }
    }
}