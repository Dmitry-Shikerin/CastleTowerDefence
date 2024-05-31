using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;

namespace Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces
{
    public interface IUiAudioSource
    {
        AudioSourceId AudioSourceId { get; }
        
        void Play();
        void SetVolume(float volume);
    }
}