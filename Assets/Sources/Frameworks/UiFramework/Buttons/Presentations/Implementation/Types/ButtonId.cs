namespace Sources.Frameworks.UiFramework.Presentation.Buttons.Types
{
    /// <summary>
    /// Нужен для определения кастомного делегата для кнопки
    /// </summary>
    public enum ButtonId
    {
        Default = 0,
        FromSettingsToHud = 1,
        FromPauseToMainMenuScene = 2,
        CompleteTutorial = 3,
        NewGame = 4,
        FromWarningNewGameToNewGame = 5,
        LeaderBoard = 6,
        LoadGame = 7,
        FromSettingsToPause = 8,
        FromLevelCompletedToMainMenuScene = 9,
        FromGameOverToMainMenuScene = 10,
    }
}