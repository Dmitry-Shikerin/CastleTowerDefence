using System.Collections.Generic;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Services.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.Services.Buttons
{
    public class UiButtonService : IUiButtonService
    {
        private readonly IButtonCommandHandler _commandHandler;

        public UiButtonService(IButtonCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Handle(IEnumerable<ButtonCommandId> commandIds)
        {
            foreach (ButtonCommandId commandId in commandIds)
                _commandHandler.Handle(commandId);
        }
    }
}