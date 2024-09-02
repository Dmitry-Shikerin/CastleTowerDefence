using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.MVPPassiveView.Controllers.Interfaces.ControllerLifetimes;
using Sources.Frameworks.MVPPassiveView.Presentations.Interfaces.PresentationsInterfaces.Views.Constructors;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Implementation.Types;
using Sources.Frameworks.UiFramework.AudioSources.Presentations.Interfaces;

namespace Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces
{
    public interface IAudioService : IInitialize, IDestroy, IConstruct<Volume>
    {
        void Play(AudioSourceId id);
        IUiAudioSource Play(AudioClipId audioClipId);
        void Play(AudioGroupId audioGroupId);
        void Stop(AudioGroupId audioGroupId);
    }
}