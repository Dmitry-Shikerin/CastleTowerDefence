using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Destroyers;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Destroyers;
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
        
        private IPODestroyerService _destroyerService = new PODestroyerService();
        private AudioSource _audioSource;
        private CancellationTokenSource _cancellationTokenSource;
        
        public AudioSourceId AudioSourceId => _audioSourceId;
        public bool IsPlaying => _audioSource.isPlaying;

        private void Awake() =>
            _audioSource = GetComponent<AudioSource>();

        private void OnEnable() =>
            _cancellationTokenSource = new CancellationTokenSource();

        private void OnDisable() =>
            _cancellationTokenSource.Cancel();

        public override void Destroy()
        {
            _destroyerService.Destroy(this);
            // if (TryGetComponent(out PoolableObject poolableObject) == false)
            // {
            //     Destroy(gameObject);
            //     Debug.Log($"PoolableObject not found: {gameObject.name}");
            //
            //     return;
            // }
            //
            // Debug.Log($"PoolableObject found: {gameObject.name}");
            // poolableObject.ReturnToPool();
            // Hide();
        }

        public async UniTask PlayAsync(Action callback)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            
            if (_audioSource == null)
                return;
            
            try
            {
                _audioSource.Play();
                await UniTask.WaitUntil(CanPlay, cancellationToken: _cancellationTokenSource.Token);
                callback?.Invoke();
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        public void StopPlayAsync() =>
            _cancellationTokenSource.Cancel();

        public void Play() =>
            _audioSource.Play();

        public IUiAudioSource SetVolume(float volume)
        {
            _audioSource.volume = volume;

            return this;
        }

        public IUiAudioSource SetClip(AudioClip clip)
        {
            if (_audioSource == null)
                return null;
            
            _audioSource.clip = clip;
            
            return this;
        }
        
        private bool CanPlay() =>
            _audioSource != null && _audioSource.clip.length <= _audioSource.time + 0.1f;
    }
}