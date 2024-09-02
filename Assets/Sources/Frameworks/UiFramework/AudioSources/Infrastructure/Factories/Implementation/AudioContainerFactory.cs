using System;
using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.Frameworks.GameServices.ObjectPools.Interfaces.Generic;
using Sources.Frameworks.UiFramework.AudioSources.Domain.Constant;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Factories.Interfaces;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;

namespace Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Factories.Implementation
{
    public class AudioContainerFactory : PoolableObjectFactory<UiAudioSource>, IAudioContainerFactory
    {
        private readonly IObjectPool<UiAudioSource> _uiAudioSourcePool;

        public AudioContainerFactory(IObjectPool<UiAudioSource> uiAudioSourcePool) 
            : base(uiAudioSourcePool)
        {
            _uiAudioSourcePool = uiAudioSourcePool ?? throw new ArgumentNullException(nameof(uiAudioSourcePool));
        }

        public UiAudioSource Create(UiAudioSource uiAudioSource)
        {
            return uiAudioSource;
        }
        
        public UiAudioSource Create()
        {
            UiAudioSource uiAudioSource = CreateView(AudioSourceConst.PrefabPath);
            
            return uiAudioSource;
        }
    }
}