using System;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation
{
    public class MainMenuSceneViewFactory : ISceneViewFactory
    {
        private readonly IAudioService _audioService;

        public MainMenuSceneViewFactory(IAudioService audioService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        }
    
        public void Create(IScenePayload payload)
        {
            //Volume
            Volume volume = new Volume("Volume");
            _audioService.Construct(volume);
        }
    }
}
