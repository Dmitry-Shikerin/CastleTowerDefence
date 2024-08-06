using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation.Handlers
{
    public class MainMenuButtonCommandHandler : IButtonCommandHandler
    {
        private readonly Dictionary<ButtonCommandId, IButtonCommand> _commands =
            new Dictionary<ButtonCommandId, IButtonCommand>();

        public MainMenuButtonCommandHandler(
            LoadMainMenuSceneCommand loadMainMenuSceneCommand,
            UnPauseButtonCommand unPauseButtonCommand,
            ShowRewardedAdvertisingButtonCommand showRewardedAdvertisingButtonCommand,
            ClearSavesButtonCommand clearSavesButtonCommand,
            NewGameCommand newGameCommand,
            ShowLeaderboardCommand showLeaderBoardCommand,
            SaveVolumeButtonCommand saveVolumeButtonCommand)
        {
            _commands[loadMainMenuSceneCommand.Id] = loadMainMenuSceneCommand;
            _commands[unPauseButtonCommand.Id] = unPauseButtonCommand;
            _commands[showRewardedAdvertisingButtonCommand.Id] = showRewardedAdvertisingButtonCommand;
            _commands[clearSavesButtonCommand.Id] = clearSavesButtonCommand;
            _commands[newGameCommand.Id] = newGameCommand;
            _commands[showLeaderBoardCommand.Id] = showLeaderBoardCommand;
            _commands[saveVolumeButtonCommand.Id] = saveVolumeButtonCommand;
        }

        public void Handle(ButtonCommandId buttonCommandId)
        {
            if (_commands.ContainsKey(buttonCommandId) == false)
                throw new KeyNotFoundException(nameof(buttonCommandId));

            _commands[buttonCommandId].Handle();
        }
    }
}