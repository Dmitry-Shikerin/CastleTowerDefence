using System;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Domain.Models.Implementation;
using Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation
{
    public class MainMenuSceneViewFactory : ISceneViewFactory
    {
        private readonly MainMenuHud _mainMenuHud;
        private readonly IAudioService _audioService;
        private readonly VolumeViewFactory _volumeViewFactory;

        public MainMenuSceneViewFactory(MainMenuHud hud,
            IAudioService audioService,
            VolumeViewFactory volumeViewFactory)
        {
            _mainMenuHud = hud ?? throw new ArgumentNullException(nameof(hud));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _volumeViewFactory = volumeViewFactory ??
                                        throw new ArgumentNullException(nameof(volumeViewFactory));
        }
    
        public void Create(IScenePayload payload)
        {
            //Volume
            Volume musicVolume = new Volume("Music");
            Volume soundsVolume = new Volume("Sounds");
            _audioService.Construct(musicVolume);

            _volumeViewFactory.Create(musicVolume, _mainMenuHud.MusicVolumeView);
            _volumeViewFactory.Create(soundsVolume, _mainMenuHud.SoundVolumeView);
        }
    }
}
