using System.Collections.Generic;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Domain.Domain;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces;
using Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Interfaces.Handlers;

namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation.Handlers
{
    public class MainMenuButtonCommandHandler : ButtonCommandHandler
    {
        public MainMenuButtonCommandHandler(
            LoadMainMenuSceneCommand loadMainMenuSceneCommand,
            UnPauseButtonCommand unPauseButtonCommand,
            ShowRewardedAdvertisingButtonCommand showRewardedAdvertisingButtonCommand,
            ClearSavesButtonCommand clearSavesButtonCommand,
            NewGameCommand newGameCommand,
            ShowLeaderboardCommand showLeaderBoardCommand,
            SaveVolumeButtonCommand saveVolumeButtonCommand,
            ShowDailyRewardViewCommand showDailyRewardViewCommand)
        {
            Add(loadMainMenuSceneCommand);
            Add(unPauseButtonCommand);
            Add(showRewardedAdvertisingButtonCommand);
            Add(clearSavesButtonCommand);
            Add(newGameCommand);
            Add(showLeaderBoardCommand);
            Add(saveVolumeButtonCommand);
            Add(showDailyRewardViewCommand);
        }
    }
}