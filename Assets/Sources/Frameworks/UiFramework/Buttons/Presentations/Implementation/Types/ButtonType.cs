namespace Sources.Frameworks.UiFramework.Presentation.Buttons.Types
{
    /// <summary>
    /// Если Default то будет переключать на указанную форму,
    /// если Custom то будет вызывать указанную функцию
    /// которую нужно добавить в FormService
    /// </summary>
    public enum ButtonType
    {
        Default = 0,
        Custom  = 1,
    }
}