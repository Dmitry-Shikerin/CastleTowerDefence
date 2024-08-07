using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation.Handlers
{
    public class GameplayButtonCommandHandler : ButtonCommandHandler
    {
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
            Add(completeTutorialCommand);
            Add(loadMainMenuSceneCommand);
            Add(unPauseButtonCommand);
            Add(showRewardedAdvertisingButtonCommand);
            Add(clearSavesButtonCommand);
            Add(newGameCommand);
            Add(showLeaderBoardCommand);
            Add(pauseButtonCommand);
        }

    }
}