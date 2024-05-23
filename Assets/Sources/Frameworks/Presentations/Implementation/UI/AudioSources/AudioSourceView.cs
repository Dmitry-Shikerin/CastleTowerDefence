using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Presentations.Views;
using Sources.PresentationsInterfaces.UI.AudioSources;
using UnityEngine;

namespace Sources.Presentations.UI.AudioSources
{
    public class AudioSourceView : View, IAudioSourceView
    {
        [Required] [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioListener _audioListener;

        public bool IsPlaying => _audioSource.isPlaying;
        public float Time => _audioSource.time;

        public void SetTime(float time) =>
            _audioSource.time = time;

        public void Mute() =>
            _audioSource.mute = true;

        public void UnMute() =>
            _audioSource.mute = false;

        public void Pause() =>
            _audioSource.Pause();

        public void UnPause() =>
            _audioSource.UnPause();

        public void Play() =>
            _audioSource.Play();

        public void SetLoop() =>
            _audioSource.loop = true;

        public void SetUnLoop() =>
            _audioSource.loop = false;

        public void Stop() =>
            _audioSource.Stop();

        public void SetClip(AudioClip audioClip) =>
            _audioSource.clip = audioClip;

        public void SetVolume(float volume) =>
            _audioSource.volume = volume;

        public async UniTask PlayToEnd(CancellationToken cancellationToken)
        {
            Play();
            await UniTask.WaitUntil(
                () => Mathf.Approximately(_audioSource.time, _audioSource.clip.length),
                cancellationToken: cancellationToken);
        }
    }
}