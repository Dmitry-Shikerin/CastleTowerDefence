using Sources.Frameworks.UiFramework.Presentation.AudioSources.Types;

namespace Sources.Frameworks.UiFramework.PresentationsInterfaces.AudioSources
{
    public interface IUiAudioSource
    {
        AudioSourceId AudioSourceId { get; }
        
        void Play();
        void SetVolume(float volume);
    }
}