using System;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Factories.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.Spawners.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;

namespace Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.Spawners.Implementation
{
    public class AudioSourceSpawner : IAudioSourceSpawner
    {
        private readonly IObjectPool<UiAudioSource> _audioSourcePool;
        private readonly IAudioContainerFactory _audioContainerFactory;

        public AudioSourceSpawner(
            IObjectPool<UiAudioSource> audioSourcePool, 
            IAudioContainerFactory audioContainerFactory)
        {
            _audioSourcePool = audioSourcePool ?? throw new ArgumentNullException(nameof(audioSourcePool));
            _audioContainerFactory = audioContainerFactory ??
                                     throw new ArgumentNullException(nameof(audioContainerFactory));
        }

        public UiAudioSource Spawn()
        {
            UiAudioSource uiAudioSource = SpawnFromPool() ?? _audioContainerFactory.Create();
            uiAudioSource.Show();
            
            return uiAudioSource;
        }
        
        private UiAudioSource SpawnFromPool()
        {
            UiAudioSource uiAudioSource = _audioSourcePool.Get<UiAudioSource>();

            if (uiAudioSource == null)
                return null;
            
            return _audioContainerFactory.Create(uiAudioSource);
        }
    }
}