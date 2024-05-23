using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons.Handlers;
using Sources.Frameworks.UiFramework.Infrastructure.Services.Buttons;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons.Handlers;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;

namespace Sources.Frameworks.UiFramework.Services.Buttons
{
    public class UiButtonService : IUiButtonService
    {
        private readonly IButtonCommandHandler _commandHandler;

        public UiButtonService(IButtonCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Handle(IEnumerable<ButtonCommandId> commandIds, IUiButton uiButton)
        {
            foreach (ButtonCommandId commandId in commandIds)
            {
                _commandHandler.Handle(uiButton, commandId);
            }
        }
    }
}