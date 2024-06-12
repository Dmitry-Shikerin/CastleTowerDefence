using System;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.GameServices.Loads.Services.Interfaces;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation
{
    public class EnableLoadGameButtonCommand : IButtonCommand
    {
        private readonly ILoadService _loadService;

        public EnableLoadGameButtonCommand(
            ILoadService loadService)
        {
            _loadService = loadService ?? throw new ArgumentNullException(nameof(loadService));
        }

        public ButtonCommandId Id => ButtonCommandId.EnableLoadGameButton;

        public void Handle()
        {
            // uiButton.Show();
            //
            // if (_loadService.HasKey(ModelId.PlayerWallet) == false)
            //     uiButton.Hide();
        }
    }
}