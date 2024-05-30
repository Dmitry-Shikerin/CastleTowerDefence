using System.Collections.Generic;
using System.Linq;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation.Handlers
{
    public class ButtonCommandHandler : IButtonCommandHandler
    {
        private readonly Dictionary<ButtonCommandId, IButtonCommand> _commands;

        public ButtonCommandHandler(params IButtonCommand[] commands)
        {
            _commands = commands.ToDictionary(command => command.Id, command => command);
        }

        public void Handle(ButtonCommandId buttonCommandId)
        {
            if (_commands.ContainsKey(buttonCommandId) == false)
                throw new KeyNotFoundException(nameof(buttonCommandId));

            _commands[buttonCommandId].Handle();
        }
    }
}