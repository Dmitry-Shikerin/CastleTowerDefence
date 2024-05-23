using Sirenix.OdinInspector;
using Sources.Frameworks.UiFramework.Domain.Constants;
using Sources.Frameworks.UiFramework.Presentation.AudioSources.Types;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.AudioSources;
using Sources.Presentations.Views;
using UnityEngine;

namespace Sources.Frameworks.UiFramework.Presentation.AudioSources
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