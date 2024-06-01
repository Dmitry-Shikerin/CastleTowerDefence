using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sources.Frameworks.GameServices.ObjectPools.Implementation;
using Sources.Frameworks.GameServices.ObjectPools.Implementation.Objects;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Configs;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Groups;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Factories.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Factories.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.Spawners.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.Spawners.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces;
using Sources.Frameworks.UiFramework.Views.Presentations.Implementation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Implementation
{
    public class AudioService : IAudioService
    {
        private readonly AudioServiceDataBase _audioServiceDataBase;
        private readonly Dictionary<AudioSourceId, IUiAudioSource> _audioSources;
        private readonly Dictionary<AudioClipId, AudioClip> _audioClips;
        private readonly Dictionary<AudioGroupId, AudioGroup> _audioGroups;
        private readonly ObjectPool<UiAudioSource> _audioSourcePool;
        private readonly IAudioContainerFactory _audioContainerFactory;
        private readonly IAudioSourceSpawner _audioSourceSpawner;

        private IVolume _volume;
        private CancellationTokenSource _audioCancellationTokenSource;

        public AudioService(
            UiCollector uiCollector,
            AudioServiceDataBase audioServiceDataBase)
        {
            _audioServiceDataBase = audioServiceDataBase ??
                                    throw new ArgumentNullException(nameof(audioServiceDataBase));

            _audioSources = uiCollector.UiAudioSources.ToDictionary(
                uiAudioSource => uiAudioSource.AudioSourceId, uiAudioSource => uiAudioSource);

            _audioClips = audioServiceDataBase.AudioClips;
            _audioGroups = audioServiceDataBase.AudioGroups;
            _audioSourcePool = new ObjectPool<UiAudioSource>();
            _audioContainerFactory = new AudioContainerFactory(_audioSourcePool);
            _audioSourceSpawner = new AudioSourceSpawner(_audioSourcePool, _audioContainerFactory);
        }

        public void Construct(IVolume volume) =>
            _volume = volume ?? throw new ArgumentNullException(nameof(volume));

        public void Initialize()
        {
            if (_volume == null)
                throw new NullReferenceException(nameof(_volume));

            _audioCancellationTokenSource = new CancellationTokenSource();
            ClearStates();
            OnVolumeChanged();
            _volume.MusicVolumeChanged += OnVolumeChanged;
        }

        public void Destroy()
        {
            _volume.MusicVolumeChanged -= OnVolumeChanged;
            _audioCancellationTokenSource.Cancel();
            ClearStates();
        }

        private void OnVolumeChanged()
        {
            foreach (IUiAudioSource audioSource in _audioSources.Values)
                audioSource.SetVolume(_volume.MusicVolume);

            foreach (UiAudioSource audioSource in _audioSourcePool.Collection)
                audioSource.SetVolume(_volume.MusicVolume);
        }

        public void Play(AudioSourceId id)
        {
            if (_audioSources.ContainsKey(id) == false)
                throw new KeyNotFoundException(id.ToString());

            _audioSources[id].Play();
        }

        public IUiAudioSource Play(AudioClipId audioClipId)
        {
            UiAudioSource audioSource = _audioSourceSpawner.Spawn();

            if (_audioClips.ContainsKey(audioClipId) == false)
                throw new KeyNotFoundException(audioClipId.ToString());

            audioSource.SetClip(_audioClips[audioClipId]);
            audioSource?.PlayAsync(() =>
            {
                if (_audioSourcePool.Collection.Count > _audioServiceDataBase.PoolCount)
                {
                    PoolableObject poolableObject = audioSource.GetComponent<PoolableObject>();
                    _audioSourcePool.RemoveFromCollection(audioSource);
                    Object.Destroy(poolableObject);
                    Debug.Log($"AudioSourceDataBase poolCount is: {_audioServiceDataBase.PoolCount}");
                }

                audioSource.Destroy();
            });

            return audioSource;
        }

        public async void PlayGroup(AudioGroupId audioGroupId)
        {
            if (_audioGroups.ContainsKey(audioGroupId) == false)
                throw new KeyNotFoundException(audioGroupId.ToString());

            if (_audioGroups[audioGroupId].IsPlaying)
                throw new InvalidOperationException($"Group {audioGroupId} is already playing");

            IUiAudioSource audioSource = _audioSourceSpawner.Spawn();
            _audioGroups[audioGroupId].Play();

            try
            {
                while (_audioCancellationTokenSource.Token.IsCancellationRequested == false &&
                       _audioGroups[audioGroupId].IsPlaying)
                {
                    foreach (AudioClip audioClip in _audioGroups[audioGroupId].AudioClips)
                    {
                        audioSource.SetClip(audioClip);
                        await audioSource.PlayAsync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                audioSource.StopPlayAsync();
            }
        }

        public void StopPlayGroup(AudioGroupId audioGroupId)
        {
            if (_audioGroups.ContainsKey(audioGroupId) == false)
                throw new KeyNotFoundException(audioGroupId.ToString());

            if (_audioGroups[audioGroupId].IsPlaying == false)
                return;

            _audioGroups[audioGroupId].Stop();
        }

        private void ClearStates()
        {
            foreach (AudioGroup audioGroup in _audioGroups.Values)
                audioGroup.Stop();
        }
    }
}