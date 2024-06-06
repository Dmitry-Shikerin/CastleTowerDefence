using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation;

namespace Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.Spawners.Interfaces
{
    public interface IAudioSourceSpawner
    {
        UiAudioSource Spawn();
    }
}