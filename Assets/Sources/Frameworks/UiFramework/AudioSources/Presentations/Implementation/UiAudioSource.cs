using Sirenix.OdinInspector;
using Sources.Frameworks.MVPPassiveView.Presentations.Implementation.Views;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces;
using Sources.Frameworks.UiFramework.Core.Domain.Constants;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation
{
    [RequireComponent(typeof(AudioSource))]
    public class UiAudioSource : View, IUiAudioSource
    {
        [DisplayAsString(false)] [HideLabel]
        [SerializeField] private string _lebel = UiConstant.UiAudioSourceLabel;
        [SerializeField] private AudioSourceId _audioSourceId;
        
        private AudioSource _audioSource;
        
        public AudioSourceId AudioSourceId => _audioSourceId;

        private void Awake() =>
            _audioSource = GetComponent<AudioSource>();

        public void Play() =>
            _audioSource.Play();

        public void SetVolume(float volume) =>
            _audioSource.volume = volume;
    }
}