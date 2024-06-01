using System;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation
{
    public class AudioServiceTester : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IAudioService _audioService;
        
        private void OnEnable() =>
            _button.onClick.AddListener(PlaySound);

        private void OnDisable() =>
            _button.onClick.RemoveListener(PlaySound);

        private void PlaySound()
        {
            _audioService.Play(AudioClipId.Button);
        }


        [Inject]
        private void Construct(IAudioService audioService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        }
    }
}