using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces;

namespace Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Factories.Interfaces
{
    public interface IAudioContainerFactory
    {
        UiAudioSource Create(UiAudioSource uiAudioSource);

        UiAudioSource Create();
    }
}