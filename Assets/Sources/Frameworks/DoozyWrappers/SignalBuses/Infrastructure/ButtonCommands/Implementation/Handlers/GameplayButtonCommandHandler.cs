﻿using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation.Handlers
{
    public class GameplayButtonCommandHandler : IButtonCommandHandler
    {
        private readonly Dictionary<ButtonCommandId, IButtonCommand> _commands =
            new Dictionary<ButtonCommandId, IButtonCommand>();

        public GameplayButtonCommandHandler(
            CompleteTutorialCommand completeTutorialCommand,
            LoadMainMenuSceneCommand loadMainMenuSceneCommand,
            UnPauseButtonCommand unPauseButtonCommand,
            PauseButtonCommand pauseButtonCommand,
            ShowRewardedAdvertisingButtonCommand showRewardedAdvertisingButtonCommand,
            ClearSavesButtonCommand clearSavesButtonCommand,
            NewGameCommand newGameCommand,
            ShowLeaderboardCommand showLeaderBoardCommand)
        {
            _commands[completeTutorialCommand.Id] = completeTutorialCommand;
            _commands[loadMainMenuSceneCommand.Id] = loadMainMenuSceneCommand;
            _commands[unPauseButtonCommand.Id] = unPauseButtonCommand;
            _commands[showRewardedAdvertisingButtonCommand.Id] = showRewardedAdvertisingButtonCommand;
            _commands[clearSavesButtonCommand.Id] = clearSavesButtonCommand;
            _commands[newGameCommand.Id] = newGameCommand;
            _commands[showLeaderBoardCommand.Id] = showLeaderBoardCommand;
            _commands[pauseButtonCommand.Id] = pauseButtonCommand;
        }

        public void Handle(ButtonCommandId buttonCommandId)
        {
            if (_commands.ContainsKey(buttonCommandId) == false)
                throw new KeyNotFoundException(nameof(buttonCommandId));

            _commands[buttonCommandId].Handle();
        }
    }
}