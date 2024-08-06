using System;
using Sources.BoundedContexts.Huds.Presentations;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.BoundedContexts.Scenes.Domain;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Domain.Implementation;
using Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Domain.Interfaces;
using Sources.Frameworks.GameServices.Volumes.Infrastucture.Factories;
using Sources.Frameworks.UiFramework.AudioSources.Infrastructure.Services.AudioService.Interfaces;
using UnityEngine;

namespace Sources.BoundedContexts.Scenes.Infrastructure.Factories.Views.Implementation
{
    public class MainMenuSceneViewFactory : ISceneViewFactory
    {
        private readonly MainMenuHud _mainMenuHud;
        private readonly ILoadService _loadService;
        private readonly IAudioService _audioService;
        private readonly MainMenuModelsLoaderService _mainMenuModelsLoaderService;
        private readonly MainMenuModelsCreatorService _mainMenuModelsCreatorService;
        private readonly VolumeViewFactory _volumeViewFactory;

        public MainMenuSceneViewFactory(
            MainMenuHud hud,
            ILoadService loadService,
            IAudioService audioService,
            MainMenuModelsLoaderService mainMenuModelsLoaderService,
            MainMenuModelsCreatorService mainMenuModelsCreatorService,
            VolumeViewFactory volumeViewFactory)
        {
            _mainMenuHud = hud ?? throw new ArgumentNullException(nameof(hud));
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            _mainMenuModelsLoaderService = mainMenuModelsLoaderService ?? throw new ArgumentNullException(nameof(mainMenuModelsLoaderService));
            _mainMenuModelsCreatorService = mainMenuModelsCreatorService ?? throw new ArgumentNullException(nameof(mainMenuModelsCreatorService));
            _volumeViewFactory = volumeViewFactory ??
                                 throw new ArgumentNullException(nameof(volumeViewFactory));
        }
    
        public void Create(IScenePayload payload)
        {
            MainMenuModel mainMenuModel = Load(payload);
            
            //Volume
            _audioService.Construct(mainMenuModel.MusicVolume);

            _volumeViewFactory.Create(mainMenuModel.MusicVolume, _mainMenuHud.MusicVolumeView);
            _volumeViewFactory.Create(mainMenuModel.SoundsVolume, _mainMenuHud.SoundVolumeView);
        }
        
        private MainMenuModel Load(IScenePayload payload)
        {
            if (_loadService.HasKey(ModelId.SoundsVolume))
            {
                Debug.Log(_loadService.HasKey(ModelId.SoundsVolume));
                Debug.Log($"Load models");
                return _mainMenuModelsLoaderService.Load();
            }
            
            return _mainMenuModelsCreatorService.Load();
        }
    }
}
