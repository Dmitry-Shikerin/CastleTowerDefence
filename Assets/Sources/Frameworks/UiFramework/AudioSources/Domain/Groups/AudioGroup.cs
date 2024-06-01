using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sources.Frameworks.UiFramework.AudioSources.Domain.Groups
{
    [Serializable]
    public class AudioGroup
    {
        [SerializeField] private List<AudioClip> _audioClips;
        [SerializeField] private float _currentTime;
        
        public IReadOnlyList<AudioClip> AudioClips => _audioClips;
        public bool IsPlaying { get; private set; } = false;
        public AudioClip CurrentClip { get; private set; }
        public float CurrentTime => _currentTime;

        public void Play() =>
            IsPlaying = true;
        
        public void Stop() =>
            IsPlaying = false;
        
        public void SetCurrentClip(AudioClip clip) =>
            CurrentClip = clip;
        
        public void SetCurrentTime(float time) =>
            _currentTime = time;
    }
}