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
        private readonly MusicChangerViewFactory _musicChangerViewFactory;
        private readonly SoundsChangerViewFactory _soundsChangerViewFactory;

        public MainMenuSceneViewFactory(MainMenuHud hud,
            IAudioService audioService,
            MusicChangerViewFactory musicChangerViewFactory,
            SoundsChangerViewFactory soundsChangerViewFactory)
        {
            _mainMenuHud = hud ?? throw new ArgumentNullException(nameof(hud));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _musicChangerViewFactory = musicChangerViewFactory ??
                                        throw new ArgumentNullException(nameof(musicChangerViewFactory));
            _soundsChangerViewFactory = soundsChangerViewFactory ??
                                        throw new ArgumentNullException(nameof(soundsChangerViewFactory));
        }
    
        public void Create(IScenePayload payload)
        {
            //Volume
            Volume volume = new Volume("Volume");
            _audioService.Construct(volume);

            _musicChangerViewFactory.Create(volume, _mainMenuHud.MusicChangerView);
            _soundsChangerViewFactory.Create(volume, _mainMenuHud.SoundsChangerView);
        }
    }
}
