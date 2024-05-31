using Sources.Frameworks.GameServices.Volumes.Domain.Models.Interfaces;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.PresentationsInterfaces.Views.Constructors;

namespace Sources.Frameworks.UiFramework.AudioSources.Services.Interfaces
{
    public interface IAudioService : IInitialize, IDestroy, IConstruct<IVolume>
    {
        void Play(AudioSourceId id);
    }
}