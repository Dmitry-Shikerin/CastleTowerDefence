using System;
using Doozy.Runtime.Signals;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Loads.Domain.Constant;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.GameServices.Scenes.Services.Interfaces;
using Sources.Frameworks.UiFramework.Core.Services.Forms.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class NewGameCommand : IButtonCommand
    {
        private readonly ILoadService _loadService;
        private readonly ISceneService _sceneService;
        private readonly IFormService _formService;

        public NewGameCommand(
            ILoadService loadService,
            ISceneService sceneService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
            _sceneService = sceneService ?? throw new ArgumentNullException(nameof(sceneService));
        }

        public ButtonCommandId Id => ButtonCommandId.NewGame;

        public void Handle()
        {
            if (_loadService.HasKey(ModelId.PlayerWallet))
            {
                Signal.Send(StreamId.MainMenu.NewGame, true);

                return;
            }
            
            _sceneService.ChangeSceneAsync(ModelId.Gameplay);
        }
    }
}