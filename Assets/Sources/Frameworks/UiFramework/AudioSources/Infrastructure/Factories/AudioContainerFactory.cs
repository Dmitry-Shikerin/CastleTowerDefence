using Sources.BoundedContexts.ObjectPools.Infrastructure.Factories;
using Sources.Frameworks.Services.ObjectPools.Generic;
using Sources.Frameworks.UiFramework.AudioSources.Domain;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces;


namespace Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Factories
{
    public class AudioContainerFactory : PoolableObjectFactory<UiAudioSource>
    {
        public AudioContainerFactory(IObjectPool<UiAudioSource> uiAudioSourcePool) 
            : base(uiAudioSourcePool)
        {
        }

        public IUiAudioSource Create(UiAudioSource uiAudioSource)
        {
            return uiAudioSource;
        }
        
        public IUiAudioSource Create()
        {
            UiAudioSource uiAudioSource = CreateView(AudioSourceConst.PrefabPath);
            
            return uiAudioSource;
        }
    }
}