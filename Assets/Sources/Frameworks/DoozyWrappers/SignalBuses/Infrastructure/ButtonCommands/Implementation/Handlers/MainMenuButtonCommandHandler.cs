namespace Sources.Frameworks.DoozyWrappers.SignalBuses.Infrastructure.ButtonCommands.Implementation.Handlers
{
    public class MainMenuButtonCommandHandler : ButtonCommandHandler
    {
        public MainMenuButtonCommandHandler(
            UnPauseButtonCommand unPauseButtonCommand,
            ShowRewardedAdvertisingButtonCommand showRewardedAdvertisingButtonCommand,
            ClearSavesButtonCommand clearSavesButtonCommand,
            NewGameCommand newGameCommand,
            ShowLeaderboardCommand showLeaderBoardCommand,
            SaveVolumeButtonCommand saveVolumeButtonCommand,
            ShowDailyRewardViewCommand showDailyRewardViewCommand,
            PlayerAccountAuthorizeButtonCommand playerAccountAuthorizeButtonCommand,
            LoadGameCommand loadGameCommand)
        {
            Add(loadGameCommand);
            Add(unPauseButtonCommand);
            Add(showRewardedAdvertisingButtonCommand);
            Add(clearSavesButtonCommand);
            Add(newGameCommand);
            Add(showLeaderBoardCommand);
            Add(saveVolumeButtonCommand);
            Add(showDailyRewardViewCommand);
            Add(playerAccountAuthorizeButtonCommand);
        }
    }
}