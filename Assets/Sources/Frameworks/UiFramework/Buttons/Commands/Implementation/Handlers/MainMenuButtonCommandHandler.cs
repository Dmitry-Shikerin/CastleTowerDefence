using System.Collections.Generic;
using Sources.Frameworks.UiFramework.Domain.Commands;
using Sources.Frameworks.UiFramework.Infrastructure.Commands.Buttons;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons;
using Sources.Frameworks.UiFramework.InfrastructureInterfaces.Commands.Buttons.Handlers;
using Sources.Frameworks.UiFramework.PresentationsInterfaces.Buttons;

namespace Sources.Frameworks.UiFramework.Buttons.Commands.Implementation.Handlers
{
    public class MainMenuButtonCommandHandler : IButtonCommandHandler
    {
        private readonly Dictionary<ButtonCommandId, IButtonCommand> _commands =
            new Dictionary<ButtonCommandId, IButtonCommand>();
        
        public MainMenuButtonCommandHandler(
            ShowFormCommand showFormCommand,
            NewGameCommand newGameCommand,
            ShowLeaderboardCommand showLeaderBoardCommand,
            EnableLoadGameButtonCommand enableLoadGameButtonCommand,
            ClearSavesButtonCommand clearSavesButtonCommand)
        {
            _commands[showFormCommand.Id] = showFormCommand;
            _commands[newGameCommand.Id] = newGameCommand;
            _commands[showLeaderBoardCommand.Id] = showLeaderBoardCommand;
            _commands[enableLoadGameButtonCommand.Id] = enableLoadGameButtonCommand;
            _commands[clearSavesButtonCommand.Id] = clearSavesButtonCommand;
        }
        
        public void Handle(IMyUiButton myUiButton, ButtonCommandId buttonCommandId)
        {
            if (_commands.ContainsKey(buttonCommandId) == false)
                throw new KeyNotFoundException(nameof(buttonCommandId));

            _commands[buttonCommandId].Handle(myUiButton);
        }
    }
}