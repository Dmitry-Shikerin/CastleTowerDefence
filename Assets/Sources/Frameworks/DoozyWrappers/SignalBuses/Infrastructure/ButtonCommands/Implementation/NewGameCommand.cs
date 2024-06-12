using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class NewGameCommand : IButtonCommand
    {
        private readonly ILoadService _loadService;
        private readonly IFormService _formService;

        public NewGameCommand(ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public ButtonCommandId Id => ButtonCommandId.NewGame;

        public void Handle()
        {
            if (_loadService.HasKey(ModelId.PlayerWallet))
            {
                Signal.Send(StreamId.MainMenu.NewGame, true);

                return;
            }

            Signal.Send(StreamId.MainMenu.NewGame, true);
        }
    }
}