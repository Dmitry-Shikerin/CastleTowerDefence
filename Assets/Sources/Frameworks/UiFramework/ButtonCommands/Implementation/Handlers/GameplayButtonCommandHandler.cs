using System.Collections.Generic;
using Sources.Frameworks.UiFramework.ButtonCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.ButtonProviders.Domain;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons;

namespace Sources.Frameworks.UiFramework.ButtonCommands.Implementation.Handlers
{
    public class GameplayButtonCommandHandler : IButtonCommandHandler
    {
        private readonly Dictionary<ButtonCommandId, IButtonCommand> _commands =
            new Dictionary<ButtonCommandId, IButtonCommand>();

        public GameplayButtonCommandHandler(
            CompleteTutorialCommand completeTutorialCommand,
            LoadMainMenuSceneCommand loadMainMenuSceneCommand,
            UnPauseButtonCommand unPauseButtonCommand,
            ShowRewardedAdvertisingButtonCommand showRewardedAdvertisingButtonCommand,
            ClearSavesButtonCommand clearSavesButtonCommand)
        {
            _commands[completeTutorialCommand.Id] = completeTutorialCommand;
            _commands[loadMainMenuSceneCommand.Id] = loadMainMenuSceneCommand;
            _commands[unPauseButtonCommand.Id] = unPauseButtonCommand;
            _commands[showRewardedAdvertisingButtonCommand.Id] = showRewardedAdvertisingButtonCommand;
            _commands[clearSavesButtonCommand.Id] = clearSavesButtonCommand;
        }

        public void Handle(ButtonCommandId buttonCommandId)
        {
            if (_commands.ContainsKey(buttonCommandId) == false)
                throw new KeyNotFoundException(nameof(buttonCommandId));

            _commands[buttonCommandId].Handle();
        }
    }
}