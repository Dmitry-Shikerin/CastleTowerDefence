using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Scenes.Domain.Implementation;
using Sources.Frameworks.GameServices.Scenes.Services.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class LoadGameCommand : IButtonCommand
    {
        private readonly ISceneService _sceneService;

        public LoadGameCommand(ISceneService sceneService)
        {
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
        }

        public ButtonCommandId Id => ButtonCommandId.LoadGame;

        public void Handle() => 
            _sceneService.ChangeSceneAsync(
                ModelId.Gameplay, new ScenePayload(ModelId.Gameplay, true, false));
    }
}