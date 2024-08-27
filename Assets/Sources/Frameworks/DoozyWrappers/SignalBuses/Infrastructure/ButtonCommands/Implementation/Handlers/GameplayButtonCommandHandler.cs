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
            SaveVolumeButtonCommand saveVolumeButtonCommand,
            NewGameCommand newGameCommand)
        {
            Add(completeTutorialCommand);
            Add(loadMainMenuSceneCommand);
            Add(unPauseButtonCommand);
            Add(showRewardedAdvertisingButtonCommand);
            Add(clearSavesButtonCommand);
            Add(newGameCommand);
            Add(pauseButtonCommand);
            Add(saveVolumeButtonCommand);
        }

    }
}