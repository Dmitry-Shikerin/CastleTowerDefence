using System;
using Doozy.Runtime.Signals;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.UiFramework.ButtonCommands.Implementation
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
                Signal.Send(StreamId.MainMenu.NewGame, false);

                return;
            }

            Signal.Send(StreamId.MainMenu.Leaderboard, false);
        }
    }
}