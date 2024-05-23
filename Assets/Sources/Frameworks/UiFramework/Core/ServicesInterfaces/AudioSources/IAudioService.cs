using Sources.ControllersInterfaces.ControllerLifetimes;
using Sources.Frameworks.UiFramework.Presentation.AudioSources.Types;

namespace Sources.Frameworks.UiFramework.ServicesInterfaces.AudioSources
{
    public interface IAudioService : IEnterable, IExitable
    {
        void Play(AudioSourceId id);
    }
}