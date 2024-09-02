using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Scenes.Domain.Implementation;
using Sources.Frameworks.GameServices.Scenes.Services.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class LoadMainMenuSceneCommand : IButtonCommand
    {
        private readonly ISceneService _sceneService;

        public LoadMainMenuSceneCommand(ISceneService sceneService)
        {
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
        }

        public ButtonCommandId Id => ButtonCommandId.LoadMainMenuScene;

        public void Handle() => 
            _sceneService.ChangeSceneAsync(
                ModelId.MainMenu, new ScenePayload(ModelId.MainMenu, false, true));
    }
}