using System;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.ServicesInterfaces.Forms;
using Sources.Frameworks.YandexSdcFramework.ServicesInterfaces.PlayerAccounts;

namespace Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons
{
    public class PlayerAccountAuthorizeButtonCommand : IButtonCommand
    {
        private readonly IFormService _formService;
        private readonly IPlayerAccountAuthorizeService _playerAccountAuthorizeService;

        public PlayerAccountAuthorizeButtonCommand(
            IFormService formService,
            IPlayerAccountAuthorizeService playerAccountAuthorizeService)
        {
            _formService = formService ?? throw new ArgumentNullException(nameof(formService));
            _playerAccountAuthorizeService = playerAccountAuthorizeService ??
                                             throw new ArgumentNullException(nameof(playerAccountAuthorizeService));
        }

        public ButtonCommandId Id => ButtonCommandId.PlayerAccountAuthorize;

        public void Handle()
        {
            // uiButton.Disable();

            _playerAccountAuthorizeService.Authorize(() =>
            {
                // uiButton.Enable();
                // _formService.Show(FormId.Hud);
            });
        }
    }
}