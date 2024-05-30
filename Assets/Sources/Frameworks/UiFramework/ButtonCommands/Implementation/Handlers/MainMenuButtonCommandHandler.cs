using System.Collections.Generic;
using Sources.Frameworks.UiFramework.ButtonCommands.Interfaces.Handlers;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Implementation;
using Sources.Frameworks.UiFramework.ButtonProviders.Infrastructure.Commands.Interfaces;
using Sources.Frameworks.UiFramework.Domain.Commands;

namespace Sources.Frameworks.UiFramework.ButtonCommands.Implementation.Handlers
{
    public class MainMenuButtonCommandHandler : IButtonCommandHandler
    {
        private readonly Dictionary<ButtonCommandId, IButtonCommand> _commands =
            new Dictionary<ButtonCommandId, IButtonCommand>();

        public MainMenuButtonCommandHandler(
            // LoadGameCommand loadGameCommand,
            NewGameCommand newGameCommand,
            ShowLeaderboardCommand showLeaderBoardCommand)
            // EnableLoadGameButtonCommand enableLoadGameButtonCommand,
            // ClearSavesButtonCommand clearSavesButtonCommand,
            // PlayerAccountAuthorizeButtonCommand playerAccountAuthorizeButtonCommand)
        {
            // _commands[loadGameCommand.Id] = loadGameCommand;
            _commands[newGameCommand.Id] = newGameCommand;
            _commands[showLeaderBoardCommand.Id] = showLeaderBoardCommand;
            // _commands[enableLoadGameButtonCommand.Id] = enableLoadGameButtonCommand;
            // _commands[clearSavesButtonCommand.Id] = clearSavesButtonCommand;
            // _commands[playerAccountAuthorizeButtonCommand.Id] = playerAccountAuthorizeButtonCommand;
        }

        public void Handle(ButtonCommandId buttonCommandId)
        {
            if (_commands.ContainsKey(buttonCommandId) == false)
                throw new KeyNotFoundException(nameof(buttonCommandId));

            _commands[buttonCommandId].Handle();
        }
    }
}