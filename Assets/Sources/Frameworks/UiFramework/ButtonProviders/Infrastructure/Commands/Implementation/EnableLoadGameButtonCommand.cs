using System;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.InfrastructureInterfaces.Services.LoadServices;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation
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